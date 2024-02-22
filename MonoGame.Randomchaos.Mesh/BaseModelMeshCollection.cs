using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonoGame.Randomchaos.Mesh
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Analogue for ModelMeshCollection. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class BaseModelMeshCollection : ReadOnlyCollection<BaseModelMesh>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="list"> The list. </param>
        ///-------------------------------------------------------------------------------------------------

        public BaseModelMeshCollection(List<BaseModelMesh> list) : base(list) { }
    }
}
