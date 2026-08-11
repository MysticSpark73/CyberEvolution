using UnityEngine;

namespace CyberEvolution.Data.Grid
{
    [CreateAssetMenu(menuName = "CyberEvolution/Data/GridData", fileName = "GridData")]
    public class GridData : ScriptableObject
    {
        public GameObject TilePrefab;
    }
}