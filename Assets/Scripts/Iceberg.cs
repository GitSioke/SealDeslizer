using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Iceberg
    {
        private IList<Vector3> _iceCubes = new List<Vector3>();


        public bool LookUpAdjdacents(Vector3 vector)
        {
            foreach(Vector3 iceVector in _iceCubes)
            {
                if((iceVector.x == vector.x && (iceVector.y == vector.y+1 || iceVector.y == vector.y-1))
                    || (iceVector.y == vector.y && (iceVector.x == vector.x+1 || iceVector.x == vector.x-1)))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddIceVector(Vector3 iceVector)
        {
            _iceCubes.Add(iceVector);
        }

        internal IEnumerable<Vector3> Ice()
        {
            return _iceCubes;
        }

        public bool ContainsIceCube(Vector3 vector)
        {
            foreach (Vector3 vectorInIceberg in _iceCubes)
            {
                if (vector.x == vectorInIceberg.x
                    && vector.y == vectorInIceberg.y)
                    return true;
            }
            return false;
        }
    }
}
