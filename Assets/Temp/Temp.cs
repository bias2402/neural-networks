using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour {
    [SerializeField] private Text reply = null;
    [SerializeField] private Text blockTip = null;
    [SerializeField] private Text socketTip = null;
    [SerializeField] private Text loopTip = null;
    private int blockTipIndex = 0;
    private int socketTipIndex = 0;
    private int loopTipIndex = 0;
    private Dictionary<int, string> blockTips = new Dictionary<int, string>();
    private Dictionary<int, string> socketTips = new Dictionary<int, string>();
    private Dictionary<int, string> loopTips = new Dictionary<int, string>();
    private Dictionary<int, string> blockTipText = new Dictionary<int, string>();
    private Dictionary<int, string> socketTipText = new Dictionary<int, string>();
    private Dictionary<int, string> loopTipText = new Dictionary<int, string>();

    void Start() {
        blockTips.Add(0, "Forskellige blokke kan findes i de tre blok kategorier");
        blockTips.Add(1, "Hvis du har en forkert blok, kan du samle den op og trykke på Slet Blok");
        blockTips.Add(2, "Du kan slette alle blokke i øverste højre hjørne");
        blockTipText.Add(0, "Hvor finder jeg blokkene?");
        blockTipText.Add(1, "Hvordan sletter jeg en blok?");
        blockTipText.Add(2, "Hvordan ryder jeg alle blokke hurtigt?");

        socketTips.Add(0, "Du kan indsætte blokke i en holder ved at klikke på den, mens du har en blok");
        socketTips.Add(1, "Du kan tage blokken fra en holder ved at klikke på den, mens du ikke har en blok");
        socketTips.Add(2, "Hvis du har en blok i en holder, kan du redigere den ved at trykke på det højre flag");
        socketTips.Add(3, "Hvis du vil kopier en blok fra en holder, kan du klikke på det venstre flag");
        socketTipText.Add(0, "Hvor skal jeg placere blokkene?");
        socketTipText.Add(1, "Hvordan fjerner jeg en blok igen?");
        socketTipText.Add(2, "Hvordan ændre jeg en blok?");
        socketTipText.Add(3, "Hvordan kan jeg få en kopi af en blok?");

        loopTips.Add(0, "Programmet starter ved den holder, hvor der står 1 nedenunder");
        loopTips.Add(1, "Pilen viser, hvilken retning programmet bliver kørt");
        loopTips.Add(2, "Tallet i midten fortæller, hvor mange gange programmet skal køres");
        loopTipText.Add(0, "Hvor starter programmet?");
        loopTipText.Add(1, "Hvilken vej køre programmet?");
        loopTipText.Add(2, "Hvad betyder tallet i midten?");

        reply.text = "";
        blockTip.text = blockTipText[blockTipIndex];
        socketTip.text = socketTipText[socketTipIndex];
        loopTip.text = loopTipText[loopTipIndex];
    }

    public void BlockTipClick() {
        if (blockTipIndex >= blockTips.Count) {
            reply.text = "Du burde kunne finde løsningen nu.";
            blockTip.text = "Ikke flere tips her";
            return;
        }
        reply.text = blockTips[blockTipIndex];
        blockTipIndex++;
        if (blockTipIndex < blockTips.Count) blockTip.text = blockTipText[blockTipIndex];
        else blockTip.text = "Ikke flere tips her";
    }

    public void SocketTipClick() {
        reply.text = socketTips[socketTipIndex];
        socketTipIndex++;
        if (socketTipIndex >= socketTips.Count) socketTipIndex = 0;
        socketTip.text = socketTipText[socketTipIndex];
    }

    public void LoopTipClick() {
        if (loopTipIndex >= loopTips.Count) {
            reply.text = "";
            for (int i = 0; i < loopTipIndex && i < loopTips.Count; i++) {
                reply.text += loopTips[i];
                reply.text += "\n";
            }
            loopTip.text = "Vil du se tips igen?";
            return;
        }
        reply.text = "";
        for (int i = 0; i <= loopTipIndex; i++) {
            reply.text += loopTips[i];
            reply.text += "\n";
        }
        loopTipIndex++;
        if (loopTipIndex < loopTips.Count) loopTip.text = loopTipText[loopTipIndex];
        else loopTip.text = "Vil du se tips igen?";
    }
}