using System.Net;

namespace Tic_Tac_Launcher;

public class DownloadAPI
{
    public string GetCurrentVersion()
    {
        WebClient webClient = new WebClient();
        string output = webClient.DownloadString("https://lp4.dev/tictacgame/version");
        if (output != null)
        {
            return output;
        }else
        {
            return "NULL";
        }
        
    }
    public string GetCurrentPatchNotes()
    {
        WebClient webClient = new WebClient();
        string output = webClient.DownloadString("https://lp4.dev/tictacgame/patchnote");
        if (output != null)
        {
            return output;
        }else
        {
            return "NULL";
        }
        
    }

    
}