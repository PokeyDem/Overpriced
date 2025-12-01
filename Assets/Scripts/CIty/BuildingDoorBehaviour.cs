using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDoorBehaviour : MonoBehaviour, IInteractibleOnClick
{
   
   private MeshRenderer _meshRenderer;
   private Material _baseMaterial1;
   private Material _baseMaterial2;
   private Material _baseMaterial3;
   private Material _baseMaterial4;
   [SerializeField] private Material _selectionMaterial;
   [SerializeField] private string _buildingDescription;
   private void Awake(){
      _meshRenderer = gameObject.GetComponent<MeshRenderer>();
      _baseMaterial1 = _meshRenderer.materials[0];
   }

   public void Interact(){
      UIManager.Instance.EnableMerchantsGuildUI();
   }

   public void InteractOnHover(){
      var newMaterials = _meshRenderer.materials;
      newMaterials[0] = _selectionMaterial;
      _meshRenderer.materials = newMaterials;
   }

   public void ResetInteractionOnHover(){
      var newMaterials = _meshRenderer.materials;
      newMaterials[0] = _baseMaterial1;
      _meshRenderer.materials = newMaterials;
   }

   public string GetBuildingDescription(){
      return _buildingDescription;
   }
}
