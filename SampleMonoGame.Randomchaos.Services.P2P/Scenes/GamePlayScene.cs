using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Extensions;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Models;
using MonoGame.Randomchaos.Primitives3D.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using MonoGame.Randomchaos.Services.P2P.Enums;
using MonoGame.Randomchaos.Services.P2P.Interfaces;
using MonoGame.Randomchaos.Services.P2P.Models;
using MonoGame.Randomchaos.UI;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;
using Newtonsoft.Json;
using SampleMonoGame.Randomchaos.Services.P2P.Enums;
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
        Dictionary<Guid, List<SphereBasicEfect>> Bullets = new Dictionary<Guid, List<SphereBasicEfect>>();
        Dictionary<Guid, List<BulletData>> BulletData = new Dictionary<Guid, List<BulletData>>();

        UILabel lblStatus;

        List<string> MessageFeed = new List<string>();

        const float Edge = 22;
        List<Vector3> StartPositions = new List<Vector3>()
        {
            new Vector3(0,0,0),
            new Vector3(Edge,0,Edge),
            new Vector3(-Edge,0,Edge),
            new Vector3(-Edge,0,-Edge),
            new Vector3(Edge,0,-Edge),
            new Vector3(Edge/2,0,Edge),
            new Vector3(Edge,0,Edge/2),
            new Vector3(Edge/2,0,-Edge),
            new Vector3(-Edge,0,Edge/2),
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

            plane.SetDirectionalLight(ld, null, Color.DarkGreen.ToVector3());

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
        }

        protected void LightGeom(CapsuleBasicEffect geom, Color diffuse)
        {
            Vector3 ld = new Vector3(1, -1, -1);
            geom.SetDirectionalLight(ld, Vector3.One * .125f, diffuse.ToVector3()) ;
        }

        protected void LightGeom(CubeBasicEffect geom, Color diffuse)
        {
            Vector3 ld = new Vector3(1, -1, -1);
            geom.SetDirectionalLight(ld, Vector3.One * .125f, diffuse.ToVector3());
        }

        protected void LightGeom(SphereBasicEfect geom, Color diffuse)
        {
            Vector3 ld = new Vector3(1, -1, -1);
            geom.SetDirectionalLight(ld, Vector3.One * .125f, diffuse.ToVector3());
        }

        public override void UnloadScene()
        {
            p2pService.OnConnectionDropped -= P2pService_OnConnectionDropped;
            p2pService.OnUdpDataReceived -= P2pService_OnUdpDataReceived;
            p2pService.OnLog -= P2pService_OnLog;

            MessageFeed.Clear();

            AvatarGuns.Clear();
            GameAvatars.Clear();
            PlayersData.Clear();
            AvatarTag.Clear();
            Bullets.Clear();
            BulletData.Clear();

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
                avatar.Transform.LocalPosition = Vector3.Zero - (Vector3.Up * .5f);
            }
            else
            {
                // Wait until I have been told who I am, then attach the camera to me.
            }

            base.LoadScene();
        }

        protected BulletData ShootBullet(Guid owner, Vector3 from, Vector3 velocity, Color color)
        {
            SphereBasicEfect bullet = new SphereBasicEfect(Game);
            bullet.Transform.Position = from;
            bullet.Transform.Scale = Vector3.One * .125f;

            bullet.Initialize();

            LightGeom(bullet, color);

            if (!Bullets.ContainsKey(owner))
            {
                Bullets.Add(owner, new List<SphereBasicEfect>());
                BulletData.Add(owner, new List<BulletData>());
            }

            Bullets[owner].Add(bullet);
            int count = BulletData[owner].Count + 1;
            BulletData data = new BulletData(owner, count, from, velocity, color);
            BulletData[owner].Add(data);

            Components.Add(bullet);

            return data;
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

        protected void SetUpAvatar(Guid id, int p, IPlayerData pd = null)
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

                if (kbManager.KeyPress(Keys.Space))
                {
                    Color col = Color.White;

                    if (p2pService.PlayerData.Properties["Color"] is Color)
                    {
                        col = (Color)p2pService.PlayerData.Properties["Color"];
                    }
                    else
                    {
                        col = JsonConvert.DeserializeObject<Color>(p2pService.PlayerData.Properties["Color"].ToString());
                    }

                    var BulletData = ShootBullet(p2pService.ClientId, camera.Transform.Position - Vector3.Up * .5f, camera.Transform.World.Forward * .25f, col);

                    p2pService.PlayerData.SetProperty("Bullet", BulletData);
                    //p2pService.Broadcast(new { });
                }

                // Keep the player in the map area.
                camera.Transform.Position = new Vector3(
                    Math.Max(-Edge, Math.Min(Edge, camera.Transform.Position.X)),
                    camera.Transform.Position.Y,
                    Math.Max(-Edge, Math.Min(Edge, camera.Transform.Position.Z))
                    );

                camera.Transform.Position = new Vector3(camera.Transform.Position.X, .5f, camera.Transform.Position.Z);
                camera.Transform.Rotation = camera.Transform.Rotation.LockRotation(new Vector3(1, 0, 1));

                
                // Tell everyone we are here :D
                Guid id = Guid.Empty;


                if (!p2pService.IsServer)
                {
                    id = p2pService.ClientId;
                }

                if (GameAvatars != null && GameAvatars.ContainsKey(id))
                {

                    var avatar = GameAvatars[id];

                    if (avatar != null)
                    {
                        p2pService.PlayerData.SetProperty("Position", camera.Transform.Position - (Vector3.One * .5f));
                        p2pService.PlayerData.SetProperty("Rotation", camera.Transform.Rotation);
                        //p2pService.PlayerData.SetProperty("Message", "My transform changed.");
                        p2pService.Broadcast(p2pService.PlayerData);

                        p2pService.PlayerData.RemoveProperty("Message");
                        p2pService.PlayerData.RemoveProperty("Bullet");

                        //AddToMessages($"Your position - {camera.Transform.Position}");
                    }
                }

                if (AvatarTag != null)
                {
                    try
                    {
                        foreach (Guid lblId in AvatarTag.Keys)
                        {
                            if (AvatarTag[lblId] != null) // may be mid setup.
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
                    }
                    catch { } // Exception thrown if the list is altered (client leaves)
                }

                if (Bullets != null)
                {
                    try
                    {
                        foreach (Guid bid in Bullets.Keys)
                        {
                            List<SphereBasicEfect> deadBullets = Bullets[bid].Where(b => Vector3.Distance(b.Transform.Position, Vector3.Zero) > 60).ToList();
                            List<int> deadDataIndex = deadBullets.Select(s => Bullets[bid].IndexOf(s)).ToList();

                            var notBulletOwner = GameAvatars.Where(w => w.Key != bid).Select(s => s.Key).ToList();

                            int idx = 0;
                            foreach (SphereBasicEfect bullet in Bullets[bid])
                            {
                                Vector3 v = BulletData[bid][idx].Velocity;

                                bullet.Transform.Position += v;

                                if (p2pService.IsServer)
                                {
                                    // Check for collision.
                                    BoundingSphere bulletBounds = new BoundingSphere(bullet.Transform.Position, bullet.Transform.Scale.X);

                                    foreach (Guid avid in notBulletOwner)
                                    {
                                        CapsuleBasicEffect avatar = GameAvatars[avid];

                                        float d = Vector3.Distance(bullet.Transform.Position, avatar.Transform.Position);

                                        BoundingBox bb = GatAvatarAABoundingBox(avatar.Transform);

                                        if (bb.Intersects(bulletBounds))
                                        {
                                            deadBullets.Add(bullet);
                                            deadDataIndex.Add(Bullets[bid].IndexOf(bullet));

                                            var shooter = PlayersData[bid];
                                            var target = PlayersData[avid];

                                            AddToMessages($"[{target.Name}] was shot by [{shooter.Name}]");

                                            p2pService.PlayerData.SetProperty("Message", $"[{target.Name}] was shot by [{shooter.Name}]");
                                            PointsData points = new PointsData(bid, 10);

                                            p2pService.PlayerData.SetProperty("Points", points);
                                            p2pService.PlayerData.SetProperty("Hit", BulletData[bid][idx]);
                                            SetPlayerScore(points);
                                            p2pService.Broadcast(p2pService.PlayerData);
                                            p2pService.PlayerData.RemoveProperty("Message");
                                            p2pService.PlayerData.RemoveProperty("Points");
                                        }
                                    }
                                }

                                idx++;
                            }

                            for (int del = 0; del < deadBullets.Count; del++)
                            {
                                Bullets[bid].Remove(deadBullets[del]);
                                BulletData[bid].RemoveAt(deadDataIndex[del]);

                                Components.Remove(deadBullets[del]);
                            }
                        }
                    }
                    catch { } // exceptions thrown if list is changed (client leaves so their bullets are removed during this loop)
                }
            }

            lblStatus.Text = "Scores:-";
            try
            {
                // Write Scores
                foreach (IPlayerData playerData in PlayersData.Values)
                {
                    lblStatus.Text += $" [{playerData.Name} - {playerData.Properties["Score"]}] ";
                }

                foreach (string msg in MessageFeed)
                {
                    lblStatus.Text += $"\n{msg}";
                }
            }
            catch { }
        }

        protected Vector3 GenerateRandomPosition()
        {
            float x, y, z;
            x = MathHelper.Lerp(-10f, 10f, rnd.NextFloat());
            y = 0;// MathHelper.Lerp(-10f, 10f, rnd.NextFloat());
            z = MathHelper.Lerp(-10f, 10f, rnd.NextFloat());

            return new Vector3(x, y, z);
        }

        private void P2pService_OnLog(LogLevelEnum lvl, string message, Exception ex = null, params object[] args)
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

                                avatar.Transform.Position = (new Vector3()).FromString(pd.Properties["Position"].ToString());
                                avatar.Transform.Rotation = JsonConvert.DeserializeObject<Quaternion>(pd.Properties["Rotation"].ToString());

                                camera.Transform.Position = (Vector3.Up * .5f) + avatar.Transform.Position;
                                camera.Transform.Rotation = avatar.Transform.Rotation;
                                avatar.Transform.Parent = camera.Transform;
                                avatar.Transform.LocalPosition = Vector3.Zero - (Vector3.Up * .5f);

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

                            // Has a bullet been fired?
                            if (pd.Properties.ContainsKey("Bullet"))
                            {
                                BulletData data = JsonConvert.DeserializeObject<BulletData>(pd.Properties["Bullet"].ToString());
                                // Spawn it.
                                ShootBullet(data.Owner, data.SourcePosition, data.Velocity, data.Color);
                            }

                            // Do we have a message?
                            if (pd.Properties.ContainsKey("Message"))
                            {
                                AddToMessages(pd.Properties["Message"].ToString());
                            }

                            if (pd.Properties.ContainsKey("Points"))
                            {
                                PointsData points = JsonConvert.DeserializeObject<PointsData>(pd.Properties["Points"].ToString());
                                SetPlayerScore(points);

                                // Remove the bullet that caused the points to change..
                                if (pd.Properties.ContainsKey("Hit"))
                                {
                                    BulletData hit = JsonConvert.DeserializeObject<BulletData>(pd.Properties["Hit"].ToString());
                                    if (BulletData.ContainsKey(hit.Owner) )
                                    {
                                        var bd = BulletData[hit.Owner].SingleOrDefault(s => s.Id == hit.Id);
                                        int idx = BulletData[hit.Owner].IndexOf(bd);

                                        BulletData[hit.Owner].Remove(bd);
                                        Components.Remove(Bullets[hit.Owner][idx]);
                                        Bullets[hit.Owner].RemoveAt(idx);
                                    }
                                }
                            }

                            break;
                    }
                }
            }
        }

        protected void SetPlayerScore(PointsData points)
        {
            IPlayerData playerData = PlayersData[points.Id];
            long score = 0;
            if (long.TryParse(playerData.Properties["Score"].ToString(), out score))
            {
                score += points.Points;
            }
            playerData.SetProperty("Score", score);
        }

        protected CapsuleBasicEffect InstanciatePlayerAvatar(Guid id, IPlayerData pd)
        {
            CapsuleBasicEffect avatar = null;
            SetUpAvatar(id, 0, pd);
            avatar = GameAvatars[id];
            avatar.Initialize();
            LightGeom(avatar, JsonConvert.DeserializeObject<Color>(pd.Properties["Color"].ToString()));
            Components.Add(avatar);

            CubeBasicEffect gun = GiveAvatarAGun(avatar);
            AvatarGuns[id] = gun;
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

            int maxMessages = 11;
            if (MessageFeed.Count > maxMessages)
            {
                MessageFeed.RemoveRange(0, MessageFeed.Count - maxMessages);
            }
        }

        private void P2pService_OnConnectionDropped(IClientPacketData client)
        {
            IPlayerData data = null;

            if (client.PlayerGameData != null)
            {
                data = JsonConvert.DeserializeObject<PlayerData>(client.PlayerGameData.ToString());
            }

            if (client.Id != p2pService.ClientId) // It me, I have been disconnected...
            {
                AddToMessages($"[{data.Name}] has disconnected...");
            }
            else
            {
                AddToMessages("You have been disconnected from by the server.");
            }

            if (client.Id == Guid.Empty)
            {
                sceneManager.LoadScene(NextScene);
            }
            else
            {
                // Remove the player from my list
                Components.Remove(AvatarGuns[client.Id]);
                Components.Remove(GameAvatars[client.Id]);

                // remove all their bullets too
                if (Bullets.ContainsKey(client.Id))
                {
                    foreach (var bullet in Bullets[client.Id])
                    {
                        Components.Remove(bullet);
                    }
                }

                // remove tags
                if (AvatarTag.ContainsKey(client.Id))
                {
                    Components.Remove(AvatarTag[client.Id]);
                }

                AvatarGuns.Remove(client.Id);

                GameAvatars.Remove(client.Id);
                PlayersData.Remove(client.Id);
                AvatarTag.Remove(client.Id);
                Bullets.Remove(client.Id);
                BulletData.Remove(client.Id);
            }
        }


        protected override void Btn_OnMouseClick(IUIBase sender, IMouseStateManager mouseState) { }

        protected BoundingBox GatAvatarAABoundingBox(ITransform transform)
        {
            Vector3 dim = new Vector3(.9f, 2, .9f);
            return (new BoundingBox(dim * -.5f, dim * .5f)).TransformedAA(transform);
        }

        GeometryLines box;

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            base.Draw(gameTime);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            if (box == null)
            {
                box = new GeometryLines(Game);
            }

            List<BoundingBox> boxes = new List<BoundingBox>();
            List<BoundingSphere> spheres = new List<BoundingSphere>();

            foreach (var key in Bullets.Keys)
            {
                foreach (var bullet in Bullets[key])
                {
                    if (spheres.Count < 2)
                    {
                        spheres.Add(new BoundingSphere(bullet.Transform.Position, bullet.Transform.Scale.X));
                    }
                }
            }

            foreach (var av in GameAvatars.Values) 
            {
                boxes.Add(GatAvatarAABoundingBox(av.Transform));
            }


            if (boxes.Count > 0)
            {
                box.DrawBoundsBoxs(boxes, Transform.Identity);
            }

            if (spheres.Count > 0) 
            {
                box.DrawBoundsSpheres(spheres, Transform.Identity);
            }
        }
    }
}
