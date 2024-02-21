
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonoGame.Randomchaos.Animation.Animation3D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Analogue of ModelMeshPartCollection. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BaseModelMeshPartCollection : ReadOnlyCollection<BaseModelMeshPart>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="list"> The list. </param>
        ///-------------------------------------------------------------------------------------------------

        public BaseModelMeshPartCollection(List<BaseModelMeshPart> list) : base(list) { }
    }
}
