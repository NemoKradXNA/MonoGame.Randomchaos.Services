
namespace MonoGame.Randomchaos.Interfaces.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A singleton. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    ///-------------------------------------------------------------------------------------------------

    public class Singleton<T> where T : class, new()
    {
        /// <summary>   The instance. </summary>
        protected static T _instance = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the instance. </summary>
        ///
        /// <value> The instance. </value>
        ///-------------------------------------------------------------------------------------------------

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();

                return _instance;
            }
        }
    }
}
