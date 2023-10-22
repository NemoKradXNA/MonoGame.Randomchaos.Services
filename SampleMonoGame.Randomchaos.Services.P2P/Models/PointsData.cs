
using System;

namespace SampleMonoGame.Randomchaos.Services.P2P.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   The points data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class PointsData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ///-------------------------------------------------------------------------------------------------

        public Guid Id { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the points. </summary>
        ///
        /// <value> The points. </value>
        ///-------------------------------------------------------------------------------------------------

        public long Points { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/10/2023. </remarks>
        ///
        /// <param name="id">       The identifier. </param>
        /// <param name="points">   The points. </param>
        ///-------------------------------------------------------------------------------------------------

        public PointsData(Guid id, long points)
        {
            Id = id;
            Points = points;
        }
    }
}
