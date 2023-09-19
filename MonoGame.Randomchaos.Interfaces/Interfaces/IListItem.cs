
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Interfaces.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for list item. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IListItem
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the data. </summary>
        ///
        /// <value> The data. </value>
        ///-------------------------------------------------------------------------------------------------

        object Data { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the display text. </summary>
        ///
        /// <value> The display text. </value>
        ///-------------------------------------------------------------------------------------------------

        string DisplayText { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the format to use. </summary>
        ///
        /// <value> The format. </value>
        ///-------------------------------------------------------------------------------------------------

        string Format { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the display value. </summary>
        ///
        /// <value> The display value. </value>
        ///-------------------------------------------------------------------------------------------------

        object DisplayValue { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the display. </summary>
        ///
        /// <value> The color of the display. </value>
        ///-------------------------------------------------------------------------------------------------

        Color DisplayColor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is enabled. </summary>
        ///
        /// <value> True if enabled, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool Enabled { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is visible. </summary>
        ///
        /// <value> True if visible, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool Visible { get; set; }
    }
}
