using UnityEngine;
using System.Diagnostics;

public class CameraCapture : MonoBehaviour
{
    public Camera targetCamera;
    public string ffmpegPath = @"C:\ffmpeg\bin\ffmpeg.exe";
    public string udpAddress = "udp://127.0.0.1:5000";
    public int width = 640;
    public int height = 480;

    private Process ffmpegProcess;
    private RenderTexture renderTexture;
    private Texture2D texture;

    void Start()
    {
        renderTexture = new RenderTexture(width, height, 24);
        texture = new Texture2D(width, height, TextureFormat.RGB24, false);

        targetCamera.targetTexture = renderTexture;

        // BURADA FİLTRE EKLENDİ
        string ffmpegArgs = $"-f rawvideo -pix_fmt rgb24 -s {width}x{height} -r 30 -i - " +
                            $"-vf hflip -f mpegts {udpAddress}"; 
        // hflip → yatay çevirme, vflip istersen hflip yerine vflip yaz

        ffmpegProcess = new Process();
        ffmpegProcess.StartInfo.FileName = ffmpegPath;
        ffmpegProcess.StartInfo.Arguments = ffmpegArgs;
        ffmpegProcess.StartInfo.UseShellExecute = false;
        ffmpegProcess.StartInfo.RedirectStandardInput = true;
        ffmpegProcess.StartInfo.CreateNoWindow = true;
        ffmpegProcess.Start();
    }

    void Update()
    {
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        byte[] frame = texture.GetRawTextureData();
        ffmpegProcess.StandardInput.BaseStream.Write(frame, 0, frame.Length);
        ffmpegProcess.StandardInput.BaseStream.Flush();
    }

    void OnApplicationQuit()
    {
        if (ffmpegProcess != null && !ffmpegProcess.HasExited)
        {
            ffmpegProcess.StandardInput.Close();
            ffmpegProcess.Kill();
        }
    }
}
