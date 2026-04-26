using UnityEngine;

public class PreviewSystem : MonoBehaviour{
     [SerializeField] private GameObject cellIndicator;
     private GameObject _previewObject;

     [SerializeField] private Material previewMaterialPrefab;
     private Material _previewMaterialInstance;

     private Renderer _cellIndicatorRenderer;

     private void Start(){
         _previewMaterialInstance = new Material(previewMaterialPrefab);
         cellIndicator.SetActive(false);
         _cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
         Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Preview"), LayerMask.NameToLayer("Player"), true);
     }

     public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size){
         _previewObject = Instantiate(prefab);
         _previewObject.layer = LayerMask.NameToLayer("Preview");
         PreparePreview();
         PrepareCursor(size);
         cellIndicator.SetActive(true);
     }

     private void PreparePreview(){
         Renderer[] renderers = _previewObject.GetComponentsInChildren<Renderer>();
         foreach (var renderer in renderers){
             Material[] materials = renderer.materials;

             for (int i = 0; i < materials.Length; i++){
                 materials[i] = _previewMaterialInstance;
             }

             renderer.materials = materials;
         }
         Collider[] colliders = _previewObject.GetComponentsInChildren<Collider>();
         foreach (var collider in colliders){
             collider.gameObject.layer = LayerMask.NameToLayer("Preview");
         }
     }

     private void PrepareCursor(Vector2Int size){
         if (size.x > 0 && size.y > 0){
             cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
             _cellIndicatorRenderer.material.mainTextureScale = size;
         }
     }

     public void StopShowingPreview(){
         cellIndicator.SetActive(false);
         Destroy(_previewObject);
     }

     public void UpdatePosition(Vector3 position, bool validity){
         cellIndicator.transform.position = position;
         _previewObject.transform.position = position;
         ApplyFeedback(validity);
     }

     private void ApplyFeedback(bool validity){
         Color color = validity ? Color.white : Color.red;
         _cellIndicatorRenderer.material.color = color;
         color.a = 0.5f;
         _previewMaterialInstance.color = color;
     }
    
}
