
namespace MonoGame.Randomchaos.ContentPipelineExtensions.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A JSON string data. </summary>
    ///
    /// <remarks>   Charles Humphrey, 04/04/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class JsonStringData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name of the game assembly. </summary>
        ///
        /// <value> The name of the game assembly. </value>
        ///-------------------------------------------------------------------------------------------------

        public string GameAssemblyName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the type reader. </summary>
        ///
        /// <value> The type reader. </value>
        ///-------------------------------------------------------------------------------------------------

        public string TypeReader { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets information describing the JSON. </summary>
        ///
        /// <value> Information describing the JSON. </value>
        ///-------------------------------------------------------------------------------------------------

        public string JsonData { get; set; }
    }
}
