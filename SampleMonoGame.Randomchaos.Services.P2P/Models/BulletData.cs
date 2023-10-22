
using Microsoft.Xna.Framework;
using System;

namespace SampleMonoGame.Randomchaos.Services.P2P.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A bullet data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BulletData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the owner. </summary>
        ///
        /// <value> The owner. </value>
        ///-------------------------------------------------------------------------------------------------

        public Guid Owner { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Id { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets source position. </summary>
        ///
        /// <value> The source position. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 SourcePosition { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the velocity. </summary>
        ///
        /// <value> The velocity. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector3 Velocity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color. </summary>
        ///
        /// <value> The color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Color { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/10/2023. </remarks>
        ///
        /// <param name="owner">    The owner. </param>
        /// <param name="id">       The identifier. </param>
        /// <param name="from">     Source for the. </param>
        /// <param name="velocity"> The velocity. </param>
        /// <param name="color">    The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public BulletData(Guid owner, int id, Vector3 from, Vector3 velocity, Color color)
        {
            Owner = owner;
            Id = id;
            Velocity = velocity;
            SourcePosition = from;
            Color = color;
        }
    }
}
