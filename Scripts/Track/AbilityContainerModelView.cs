using System;
using System.Collections;
using UnityEngine;

namespace Track
{
    public class AbilityContainerModelView : MonoBehaviour
    {
        [SerializeField]
        private float recoveryTime;
        [SerializeField]
        private GameObject containerMesh;

        private float hidedTime;
        private bool isActive = true;

        
        
        public IAbility Ability { get; private set; }

        private void Start()
        {
            CreateAbility();
        }

        private void CreateAbility()
        {
            Ability = AbilityFactory.CreateRandomAbility();
            containerMesh = Instantiate(Ability.Data.ContainerMesh, transform);

        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (isActive)
            {
                if (other.TryGetComponent(out ShipModelView shipMV))
                {
                    if(Ability is IPrimary)
                        shipMV.PrimaryAbility = Ability;

                    if (Ability is ISecondary)
                        shipMV.SecondaryAbility = Ability;
                    
                    HideContainer();
                }
            }
        }

        private void HideContainer()
        {
            isActive = false;
            Destroy(containerMesh);
            hidedTime = Time.time;
        }

        private void FixedUpdate()
        {
            if (isActive == false)
            {
                if (Time.time > hidedTime + recoveryTime)
                {
                    isActive = true;
                    CreateAbility();
                }
            }
        }
    }
}