using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour{
     [SerializeField] private GameObject _cellIndicator;
     private GameObject _previewObject;

     [SerializeField] private Material _previewMaterialPrefab;
     private Material previewMaterialInstance;

     private Renderer _cellIndicatorRenderer;

     private void Start(){
         previewMaterialInstance = new Material(_previewMaterialPrefab);
         _cellIndicator.SetActive(false);
         _cellIndicatorRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
     }

     public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size){
         _previewObject = Instantiate(prefab);
         _previewObject.GetComponentInChildren<BoxCollider>().enabled = false;
         PreparePreview();
         PrepareCursor(size);
         _cellIndicator.SetActive(true);
     }

     private void PreparePreview(){
         Renderer[] renderers = _previewObject.GetComponentsInChildren<Renderer>();
         foreach (var renderer in renderers){
             Material[] materials = renderer.materials;

             for (int i = 0; i < materials.Length; i++){
                 materials[i] = previewMaterialInstance;
             }

             renderer.materials = materials;
         }
     }

     private void PrepareCursor(Vector2Int size){
         if (size.x > 0 && size.y > 0){
             _cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
             _cellIndicatorRenderer.material.mainTextureScale = size;
         }
     }

     public void StopShowingPreview(){
         _cellIndicator.SetActive(false);
         Destroy(_previewObject);
     }

     public void UpdatePosition(Vector3 position, bool validity){
         _cellIndicator.transform.position = position;
         _previewObject.transform.position = position;
         ApplyFeedback(validity);
     }

     private void ApplyFeedback(bool validity){
         Color color = validity ? Color.white : Color.red;
         _cellIndicatorRenderer.material.color = color;
         color.a = 0.5f;
         previewMaterialInstance.color = color;
     }
    
}
