using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Extensions;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Primitives3D.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.UI;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;
using Newtonsoft.Json;
using SampleMonoGame.Randomchaos.Services.P2P.Enums;
using SampleMonoGame.Randomchaos.Services.P2P.Interfaces;
using SampleMonoGame.Randomchaos.Services.P2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleMonoGame.Randomchaos.Services.P2P.Scenes
{
    public class GamePlayScene : P2PBaseScene
    {
        string NextScene = "mainMenu";

        Dictionary<Guid, CapsuleBasicEffect> GameAvatars = new Dictionary<Guid, CapsuleBasicEffect>();
        Dictionary<Guid, CubeBasicEffect> AvatarGuns = new Dictionary<Guid, CubeBasicEffect>();
        Dictionary<Guid, UILabel> AvatarTag = new Dictionary<Guid, UILabel>();
        Dictionary<Guid, IPlayerData> PlayersData = new Dictionary<Guid, IPlayerData>();
        List<CubeBasicEffect> Map = new List<CubeBasicEffect>();


        UILabel lblStatus;

        List<string> MessageFeed = new List<string>()
        {
            "Game On!"
        };

        const float Edgge = 19;
        List<Vector3> StartPositions = new List<Vector3>()
        {
            new Vector3(0,0,0),
            new Vector3(Edgge,0,Edgge),
            new Vector3(-Edgge,0,Edgge),
            new Vector3(-Edgge,0,-Edgge),
            new Vector3(Edgge,0,-Edgge),
            new Vector3(Edgge/2,0,Edgge),
            new Vector3(Edgge,0,Edgge/2),
            new Vector3(Edgge/2,0,-Edgge),
            new Vector3(-Edgge,0,Edgge/2),
        };

        public GamePlayScene(Game game, string name) : base(game, name) { UIComponentTypes.Add(typeof(UIBase)); }

        Random rnd = new Random(DateTime.UtcNow.Millisecond);

        public override void Initialize()
        {

            font = Game.Content.Load<SpriteFont>("Fonts/font");

            PlaneBasicEffect plane = new PlaneBasicEffect(Game);
            plane.Transform.Scale = Vector3.One * 5;
            plane.Transform.Position = new Vector3(0, -1, -.5f);
            Components.Add(plane);

            foreach (CubeBasicEffect block in Map)
            {
                Components.Add(block);
            }

            if (p2pService.IsServer)
            {
                foreach (Guid id in GameAvatars.Keys)
                {
                    Components.Add(AvatarGuns[id]);
                    Components.Add(GameAvatars[id]);

                    AvatarTag[id].Font = font;
                    Components.Add(AvatarTag[id]);
                }
            }

            // Then do UI.
            Point btnSize = new Point(256, font.LineSpacing + 12);
            Texture2D txtBg = new Texture2D(GraphicsDevice, 512, 38);

            txtBg.FillWithColor(new Color(.1f, .1f, .1f, .75f));

            lblStatus = new UILabel(Game)
            {
                Font = font,
                Position = new Point(32, GraphicsDevice.Viewport.Height - 300),
                Text = $"",
                Tint = Color.Black,
                Background = txtBg,
                TextAlingment = TextAlingmentEnum.LeftTop,
                TextPositionOffset = new Vector2(8, 8),
                Size = new Point(1024, 256),
            };

            Components.Add(lblStatus);

            base.Initialize();

            Vector3 ld = new Vector3(1, -1, -1);

            plane.SetDirectionalLight(ld,null, Color.DarkGreen.ToVector3());

            foreach (Guid id in GameAvatars.Keys)
            {
                PlayerData pd = (PlayerData)PlayersData[id];

                Color c;

                if (pd.Properties["Color"] is Color)
                {
                    c = (Color)pd.Properties["Color"];
                }
                else
                {
                    c = JsonConvert.DeserializeObject<Color>(pd.Properties["Color"].ToString());
                }
                LightGeom(GameAvatars[id], c);
                LightGeom(AvatarGuns[id], c);
                AvatarTag[id].Tint = c;
            }

            foreach (CubeBasicEffect block in Map)
            {
                LightGeom(block, Color.SaddleBrown);
            }

        }

        protected void LightGeom(CapsuleBasicEffect geom, Color diffuse)
        {
            Vector3 ld = new Vector3(1, -1, -1);
            geom.SetDirectionalLight(ld, Vector3.One * .25f, diffuse.ToVector3()) ;
        }

        protected void LightGeom(CubeBasicEffect geom, Color diffuse)
        {
            Vector3 ld = new Vector3(1, -1, -1);
            geom.SetDirectionalLight(ld, Vector3.One * .25f, diffuse.ToVector3());
        }

        public override void UnloadScene()
        {
            p2pService.OnConnectionDropped -= P2pService_OnConnectionDropped;
            p2pService.OnUdpDataReceived -= P2pService_OnUdpDataReceived;
            p2pService.OnLog -= P2pService_OnLog;

            Map.Clear();
            AvatarGuns.Clear();
            GameAvatars.Clear();
            PlayersData.Clear();
            AvatarTag.Clear();

            if (p2pService.IsServer)
            {
                p2pService.StopServer();
            }
            else
            {
                p2pService.Disconnect();
            }

            base.UnloadScene();
        }

        public override void LoadScene()
        {
            p2pService.OnConnectionDropped += P2pService_OnConnectionDropped;
            p2pService.OnUdpDataReceived += P2pService_OnUdpDataReceived;
            p2pService.OnLog += P2pService_OnLog;

            var clients = p2pService.Clients; // p2pService.GetClientsPlayerGameData<PlayerData>();

            Texture2D map = Game.Content.Load<Texture2D>("Textures/gameMap");
            Color[] mpc = new Color[map.Width * map.Height];
            map.GetData(mpc);

            // Build map.
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 40; y++)
                {
                    Vector3 pos = new Vector3(-20 + x, 1.25f, -20 + y);
                    Point uv = new Point(x / 2, y / 2);

                    if (mpc[uv.X + (uv.Y * map.Width)].A > 0)
                    {
                        CubeBasicEffect block = new CubeBasicEffect(Game);
                        block.Transform.Scale = new Vector3(1, 5, 1);
                        block.Transform.Position = pos;

                        //Map.Add(block);
                    }
                }
            }

            if (p2pService.IsServer)
            {
                // Give everyone there positions... and start the game...
                int p = 0;
                foreach (IClientPacketData client in clients)
                {
                    SetUpAvatar(client.Id, p++);

                    CubeBasicEffect gun = GiveAvatarAGun(GetAvatarById(client.Id));
                    AvatarGuns[client.Id] = gun;
                    AvatarTag[client.Id] = AvatarLabel();
                }

                // Add the server.
                SetUpAvatar(Guid.Empty, p);

                var avatar = GetAvatarById(Guid.Empty);
                AvatarGuns[Guid.Empty] = GiveAvatarAGun(avatar);
                AvatarTag[Guid.Empty] = AvatarLabel();


                camera.Transform.Position = (Vector3.Up * .5f) + avatar.Transform.Position;
                camera.Transform.Rotation = avatar.Transform.Rotation;
                avatar.Transform.Parent = camera.Transform;
                avatar.Transform.LocalPosition -= (Vector3.Up * .5f);
            }
            else
            {
                // Wait until I have been told who I am, then attach the camera to me.
            }

            base.LoadScene();
        }

        protected UILabel AvatarLabel()
        {
            return new UILabel(Game) { Font = font, Tint = Color.Black, ShadowColor = Color.Black, ShadowOffset = new Vector2(1, 1) };
        }

        protected CapsuleBasicEffect GetAvatarById(Guid id)
        {
            var v = GameAvatars.SingleOrDefault(s => s.Key == id).Value;

            if (v != null)
            {
                return v;
            }

            return null;
        }

        protected CubeBasicEffect GiveAvatarAGun(CapsuleBasicEffect clientAvatar)
        {
            CubeBasicEffect gun = new CubeBasicEffect(Game);

            gun.Transform.Parent = clientAvatar.Transform;
            gun.Transform.LocalScale = new Vector3(.1f, .1f, 1f);
            gun.Transform.LocalPosition = new Vector3(0, 0, -1f);

            return gun;
        }

        protected void SetUpAvatar(Guid id, int p, PlayerData pd = null)
        {
            CapsuleBasicEffect clientAvatar = new CapsuleBasicEffect(Game);
            
            float r, g, b;
            r = MathHelper.Lerp(.5f, 1f, rnd.NextFloat());
            g = MathHelper.Lerp(.5f, 1f, rnd.NextFloat());
            b = MathHelper.Lerp(.5f, 1f, rnd.NextFloat());

            Color c = new Color(r, g, b, 1f);

            if (pd == null)
            {
                if (id == Guid.Empty)
                {
                    pd = p2pService.PlayerData;
                }
                else
                {
                    var client = p2pService.Clients.SingleOrDefault(s => s.Id == id);
                    pd = JsonConvert.DeserializeObject<PlayerData>(client.PlayerGameData.ToString());
                }
            }

            Vector3 pos = StartPositions[p];

            clientAvatar.Transform.Position = pos;
            
            if (!pd.Properties.ContainsKey("Color"))
            {
                pd.SetProperty("Color", c);
                pd.SetProperty("Score", 0);
                pd.SetProperty("Position", pos);
                pd.SetProperty("Rotation", Quaternion.Identity);
            }
            else
            {
                // It's already been set.
            }           

            GameAvatars.Add(id, clientAvatar);
            AvatarGuns.Add(id, null);
            AvatarTag.Add(id, null);
            p++;

            if (p >= StartPositions.Count)
            {
                p = 0;
            }

            PlayersData.Add(id, pd);
        }

        public override void Update(GameTime gameTime)
        {
            if (State == SceneStateEnum.Loaded)
            {

                if (p2pService.IsServer)
                {
                    var toSetup = PlayersData.Where(c => (long)c.Value.Properties["Status"] == (long)ThisGamesStateEnum.ClientReady).ToList();
                    foreach (var client in toSetup)
                    {
                        p2pService.SendDataTo(client.Key, client.Value);
                    }
                }

                if (kbManager.KeyPress(Keys.Escape))
                {
                    sceneManager.LoadScene(NextScene);
                }
                // Camera controls..
                float speedTran = .1f;
                float speedRot = .01f;

                Vector3 lastPos = camera.Transform.Position;
                Quaternion rot = camera.Transform.Rotation;

                if (kbManager.KeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                    camera.Transform.Translate(Vector3.Forward * speedTran);
                if (kbManager.KeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                    camera.Transform.Translate(Vector3.Backward * speedTran);
                if (kbManager.KeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                    camera.Transform.Translate(Vector3.Left * speedTran);
                if (kbManager.KeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                    camera.Transform.Translate(Vector3.Right * speedTran);

                if (kbManager.KeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0)
                    camera.Transform.Rotate(Vector3.Up, speedRot);
                if (kbManager.KeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0)
                    camera.Transform.Rotate(Vector3.Up, -speedRot);
                if (kbManager.KeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0)
                    camera.Transform.Rotate(Vector3.Right, speedRot);
                if (kbManager.KeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < 0)
                    camera.Transform.Rotate(Vector3.Right, -speedRot);

                camera.Transform.Position = new Vector3(camera.Transform.Position.X, .5f, camera.Transform.Position.Z);
                camera.Transform.Rotation = camera.Transform.Rotation.LockRotation(new Vector3(1,0,1));

                // Do I bump into the map?
                BoundingBox myBB = (new BoundingBox(-Vector3.One*.5f, Vector3.One*.5f)).Transformed(camera.Transform);

                // Get nearest blocks
                // Sort collisiion.
                //var blocks = Map.Where(w => Vector3.Distance(w.Transform.Position, camera.Transform.Position) <= 2);
                //foreach (var block in blocks)
                //{
                //    BoundingBox blockBB = (new BoundingBox(-Vector3.One*.25f, Vector3.One*.25f)).Transformed(block.Transform);
                //    if (blockBB.Intersects(myBB))
                //    {
                //        Vector3 r = Vector3.Normalize(new Vector3(block.Transform.Position.X, camera.Transform.Position.Y, block.Transform.Position.Y) - camera.Transform.Position);
                //        Vector3 n = new Vector3((int)block.Transform.Position.X < (int)camera.Transform.Position.X ? -1 : (int)block.Transform.Position.X > (int)camera.Transform.Position.X ? 1 : 0, 0, 
                //            (int)block.Transform.Position.Z < (int)camera.Transform.Position.Z ? -1 : (int)block.Transform.Position.Z > (int)camera.Transform.Position.Z ? 1 : 0);
                //        r = Vector3.Normalize(Vector3.Reflect(r, n));
                //        camera.Transform.Position = lastPos;
                //        break;
                //    }
                //}

                // Tell everyone we are here :D
                Guid id = Guid.Empty;

                if (!p2pService.IsServer)
                {
                    id = p2pService.ClientId;
                }

                if (GameAvatars.ContainsKey(id))
                {

                    var avatar = GameAvatars[id];

                    if (avatar != null)
                    {
                        p2pService.PlayerData.SetProperty("Position", camera.Transform.Position - (Vector3.One * .5f));
                        p2pService.PlayerData.SetProperty("Rotation", camera.Transform.Rotation);
                        p2pService.PlayerData.SetProperty("Message", "My transform changed.");
                        p2pService.Broadcast(p2pService.PlayerData);

                        AddToMessages($"Your position - {camera.Transform.Position}");
                    }
                }

                foreach (Guid lblId in AvatarTag.Keys)
                {
                    if (lblId != id)
                    {
                        AvatarTag[lblId].Position = camera.WorldPositionToScreenPosition(GameAvatars[lblId].Transform.Position).ToPoint();
                        AvatarTag[lblId].Text = PlayersData[lblId].Name;
                    }
                    else
                    {
                        AvatarTag[lblId].Visible = AvatarTag[lblId].Enabled = false;
                    }
                }
            }

            lblStatus.Text = "";
            try
            {
                foreach (string msg in MessageFeed)
                {
                    lblStatus.Text += $"{msg}\n";
                }
            }
            catch { }

            base.Update(gameTime);
        }

        public Vector2 Get2DCoords(GraphicsDevice GraphicsDevice, Vector3 myPosition, ICameraService Camera)
        {
            Matrix ViewProjectionMatrix = Camera.View * Camera.Projection;

            Vector4 result4 = Vector4.Transform(myPosition, ViewProjectionMatrix);

            Viewport vp = GraphicsDevice.Viewport;// Camera.Viewport;

            if (result4.W <= 0)
                return new Vector2(vp.Width, 0);

            Vector3 result = new Vector3(result4.X / result4.W, result4.Y / result4.W, result4.Z / result4.W);

            Vector2 retVal = new Vector2((int)Math.Round(+result.X * (vp.Width / 2)) + (vp.Width / 2), (int)Math.Round(-result.Y * (vp.Height / 2)) + (vp.Height / 2));
            return retVal;
        }

        protected Vector3 GenerateRandomPosition()
        {
            float x, y, z;
            x = MathHelper.Lerp(-10f, 10f, rnd.NextFloat());
            y = 0;// MathHelper.Lerp(-10f, 10f, rnd.NextFloat());
            z = MathHelper.Lerp(-10f, 10f, rnd.NextFloat());

            return new Vector3(x, y, z);
        }

        private void P2pService_OnLog(Enums.LogLevelEnum lvl, string message, Exception ex = null, params object[] args)
        {
            // log it.
        }

        private void P2pService_OnUdpDataReceived(ICommsPacket pkt)
        {
            if (pkt.Data != null)
            {
                PlayerData pd = JsonConvert.DeserializeObject<PlayerData>(pkt.Data.ToString());

                if (pd != null)
                {
                    ThisGamesStateEnum state = ThisGamesStateEnum.None;
                    CapsuleBasicEffect avatar = null;

                    if (pd.Properties.ContainsKey("Status"))
                    {
                        state = (ThisGamesStateEnum)(long)pd.Properties["Status"];
                    }

                    switch (state)
                    {
                        case ThisGamesStateEnum.ClientReady: // got my setup data.

                            if (!p2pService.IsServer && !GameAvatars.ContainsKey(p2pService.ClientId))
                            {
                                p2pService.PlayerData = pd;
                                p2pService.PlayerData.SetProperty("Status", ThisGamesStateEnum.InGame);
                                avatar = InstanciatePlayerAvatar(p2pService.ClientId, p2pService.PlayerData);

                                camera.Transform.Position = (Vector3.Up * .5f) + avatar.Transform.Position;
                                camera.Transform.Rotation = avatar.Transform.Rotation;
                                avatar.Transform.Parent = camera.Transform;
                                avatar.Transform.LocalPosition -= (Vector3.Up * .5f);

                                // Tell caller I am set.
                                AddToMessages($"You have joined the game.");
                            }
                            break;
                        case ThisGamesStateEnum.InGame: // Position update
                            
                            // Find their avatar
                            if (!GameAvatars.ContainsKey(pkt.Id))
                            {
                                avatar = InstanciatePlayerAvatar(pkt.Id,pd);
                                AddToMessages($"[{pd.Name}] has joined the game.");
                            }

                            if (avatar == null)
                            {
                                avatar = GameAvatars[pkt.Id];
                            }

                            avatar.Transform.Position = (new Vector3()).FromString(pd.Properties["Position"].ToString());
                            avatar.Transform.Rotation = JsonConvert.DeserializeObject<Quaternion>(pd.Properties["Rotation"].ToString());

                            AddToMessages($"[{pd.Name}] - {avatar.Transform.Position}");

                            break;
                    }
                }
            }
        }

        protected CapsuleBasicEffect InstanciatePlayerAvatar(Guid id, PlayerData pd)
        {
            CapsuleBasicEffect avatar = null;
            SetUpAvatar(id, 0, pd);
            avatar = GameAvatars[id];
            avatar.Initialize();
            LightGeom(avatar, JsonConvert.DeserializeObject<Color>(pd.Properties["Color"].ToString()));
            Components.Add(avatar);

            CubeBasicEffect gun = GiveAvatarAGun(avatar);
            gun.Initialize();
            LightGeom(gun, JsonConvert.DeserializeObject<Color>(pd.Properties["Color"].ToString()));
            Components.Add(gun);

            AvatarTag[id] = AvatarLabel();
            AvatarTag[id].Tint = JsonConvert.DeserializeObject<Color>(pd.Properties["Color"].ToString());
            AvatarTag[id].Initialize();
            Components.Add(AvatarTag[id]);

            return avatar;
        }

        private void AddToMessages(string msg)
        {
            MessageFeed.Add(msg);

            if (MessageFeed.Count > 13)
            {
                MessageFeed.RemoveRange(0, MessageFeed.Count - 13);
            }
        }

        private void P2pService_OnConnectionDropped(IClientPacketData client)
        {
            IPlayerData data = null;

            if (client.PlayerGameData != null)
            {
                data = JsonConvert.DeserializeObject<PlayerData>(client.PlayerGameData.ToString());
            }

            if (client.Id == Guid.Empty)
            {
                sceneManager.LoadScene(NextScene);
            }
        }


        protected override void Btn_OnMouseClick(IUIBase sender, IMouseStateManager mouseState) { }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            base.Draw(gameTime);
        }
    }
}
