using UnityEngine;
using UnityEngine.Rendering.Universal; // Für RendererFeature

// Sie könnten auch ein ScriptableRendererFeature erstellen für mehr Integration
// Aber für den Anfang ist ein Skript, das OnRenderImage (für Built-in RP) oder CommandBuffers (für URP) nutzt, einfacher.

// Für URP ist es am besten, ein ScriptableRendererFeature zu verwenden.
// Hier ist ein vereinfachtes Beispiel, das direkt in der Kamera attachiert werden kann,
// aber für eine saubere URP-Integration ist das ScriptableRendererFeature der Weg.

[RequireComponent(typeof(Camera))]
public class SelectiveFogEffect : MonoBehaviour
{
    public Material fogMaterial; // Ihr CustomFogShader Material
    public LayerMask affectedLayers; // Optional: welche Objekte sollen vom Nebel betroffen sein?

    private Camera _camera;

    void OnEnable()
    {
        _camera = GetComponent<Camera>();
        if (fogMaterial == null)
        {
            Debug.LogError("Fog Material is not assigned to SelectiveFogEffect on camera " + gameObject.name);
            enabled = false;
            return;
        }
    }

    // OnRenderImage ist für Built-in Render Pipeline. Für URP sollten Sie Command Buffers nutzen
    // oder ein ScriptableRendererFeature erstellen.
    // Das ist der "einfachste" Weg, um einen Post-Processing-Effekt direkt an einer Kamera anzubringen,
    // aber nicht der "korrekte" URP-Weg.

    // ----- Korrekter URP-Weg (ScriptableRendererFeature) wird unten beschrieben -----

    // Dies ist eine "Quick-and-Dirty"-Lösung für Tests, die nicht gut mit URP skaliert.
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (fogMaterial != null)
        {
            Graphics.Blit(source, destination, fogMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    // Optional: Wenn Sie wirklich spezifische Layer ausschließen wollen,
    // müsste der Shader den Depth-Buffer lesen und basierend auf der World-Position
    // von Objekten in diesen Layern den Nebel nicht anwenden.
    // Dies wird SEHR komplex und ist oft nicht der beste Ansatz.
    // Meistens ist es einfacher, den Nebel auf alle Objekte anzuwenden und nur bestimmte Kameras sehen ihn.
}
