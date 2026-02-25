using System;
using UnityEngine;

namespace Player
{
    public class InstantiatingBullet : MonoBehaviour
    {
        [Header("BulletPrefabs")] 
        public GameObject BulletPrefabs;

        [Header("Transform")] 
        public Transform canonPos;
        
        private void Start()
        {
           CreateBullet();
        }

        private void CreateBullet()
        {
            //Instantiate(BulletPrefabs,canonPos )
            
        }
    }
    
}
