using UnityEngine;

namespace Tofunaut.Bootstrap.UnityModels
{
    public class GridModel : UnityModel<Grid>
    {
        public Vector3 CellGap = Vector3.zero;
        public GridLayout.CellLayout CellLayout = GridLayout.CellLayout.Rectangle;
        public Vector3 CellSize = Vector3.one;
        public GridLayout.CellSwizzle CellSwizzle = GridLayout.CellSwizzle.XYZ;
        
        public override Grid Build()
        {
            var toReturn = BuildGameObject(typeof(Grid)).GetComponent<Grid>();
            toReturn.cellGap = CellGap;
            toReturn.cellLayout = CellLayout;
            toReturn.cellSize = CellSize;
            toReturn.cellSwizzle = CellSwizzle;

            return toReturn;
        }
    }
}