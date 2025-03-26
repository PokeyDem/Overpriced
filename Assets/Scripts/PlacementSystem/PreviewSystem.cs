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
         Physics.IgnoreLayerCollision(8, 9, true);
     }

     public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size){
         _previewObject = Instantiate(prefab);
         _previewObject.layer = 8;
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
         Collider[] colliders = _previewObject.GetComponentsInChildren<Collider>();
         foreach (var collider in colliders){
             collider.gameObject.layer = 8;
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
