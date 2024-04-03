
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Interfaces.Interfaces;

namespace Samples.MonoGame.Randomchaos.UI.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A list item. </summary>
    ///
    /// <remarks>   Charles Humphrey, 03/04/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ListItem : IListItem
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the data. </summary>
        ///
        /// <value> The data. </value>
        ///-------------------------------------------------------------------------------------------------

        public object Data { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the display text. </summary>
        ///
        /// <value> The display text. </value>
        ///-------------------------------------------------------------------------------------------------

        public string DisplayText { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the format to use. </summary>
        ///
        /// <value> The format. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Format { get; set; } = "{0}";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the display value. </summary>
        ///
        /// <value> The display value. </value>
        ///-------------------------------------------------------------------------------------------------

        public object DisplayValue { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of the display. </summary>
        ///
        /// <value> The color of the display. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color DisplayColor { get; set; } = Color.Black;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is enabled. </summary>
        ///
        /// <value> True if enabled, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Enabled { get; set; } = true;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is visible. </summary>
        ///
        /// <value> True if visible, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Visible { get; set; } = true;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 03/04/2024. </remarks>
        ///
        /// <param name="text">     The text. </param>
        /// <param name="color">    The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public ListItem(string text, Color color)
        {
            DisplayText = text;
            DisplayColor = color;
        }
    }
}
