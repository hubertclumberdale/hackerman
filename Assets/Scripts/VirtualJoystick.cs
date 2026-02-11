using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
   [Header("Output")]
   public Vector2 movement = Vector2.zero;
   
   [Header("Settings")]
   public float moveRadius = 50f;
   public float returnSpeed = 8f;
   
   private RectTransform rectTransform;
   private Canvas parentCanvas;
   private Vector2 centerPosition;
   private bool isDragging = false;
   private Vector2 dragStartPosition; // Posizione iniziale del drag
   private Vector2 joystickStartPosition; // Posizione iniziale del joystick
   
   void Start()
   {
      rectTransform = GetComponent<RectTransform>();
      parentCanvas = GetComponentInParent<Canvas>();
      
      // Il centro è la posizione iniziale di questo RectTransform
      centerPosition = rectTransform.anchoredPosition;
   }
   
   void Update()
   {
      if (!isDragging)
      {
         // Torna al centro quando non è premuto
         rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, centerPosition, returnSpeed * Time.deltaTime);
         
         // Reset movimento se è molto vicino al centro
         if (Vector2.Distance(rectTransform.anchoredPosition, centerPosition) < 1f)
         {
            movement = Vector2.zero;
            rectTransform.anchoredPosition = centerPosition;
         }
      }
   }
   
   public void OnPointerDown(PointerEventData eventData)
   {
      isDragging = true;
      
      // Salva la posizione di partenza del drag in coordinate schermo
      dragStartPosition = eventData.position;
      joystickStartPosition = rectTransform.anchoredPosition;
   }
   
   public void OnPointerUp(PointerEventData eventData)
   {
      isDragging = false;
      movement = Vector2.zero;
   }
   
   public void OnDrag(PointerEventData eventData)
   {
      if (!isDragging) return;
      
      // Calcola il delta del movimento in coordinate schermo
      Vector2 dragDelta = eventData.position - dragStartPosition;
      
      // Converti il delta in coordinate locali del Canvas
      Vector2 localDelta = dragDelta / parentCanvas.scaleFactor;
      
      // Calcola la nuova posizione del joystick
      Vector2 newPosition = joystickStartPosition + localDelta;
      
      // Calcola la direzione dal centro
      Vector2 direction = newPosition - centerPosition;
      
      // Limita entro il raggio
      if (direction.magnitude > moveRadius)
      {
         direction = direction.normalized * moveRadius;
         newPosition = centerPosition + direction;
      }
      
      // Muovi il joystick
      rectTransform.anchoredPosition = newPosition;
      
      // Calcola l'output del movimento (normalizzato)
      movement = direction / moveRadius;
   }
   
   // Metodi di compatibilità
   public float GetAxis(string axisName)
   {
      if (axisName == "Horizontal")
         return movement.x;
      if (axisName == "Vertical")
         return movement.y;
      return 0f;
   }
   
   public float Horizontal { get { return movement.x; } }
   public float Vertical { get { return movement.y; } }
   public Vector2 Direction { get { return movement; } }
}