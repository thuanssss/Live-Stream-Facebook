using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using YoutubeExtractor;

namespace LiveStreamFacebook.Models
{
    public class ServiceYoutube
    {
        private string LinkYoutube { get; set; }
        private int FormatType { get; set; }
        private string FullLinkDownload { get; set; }
        private HttpServerUtilityBase Server { get; set; }

        private const string PathFfmpeg = @"~\Contents\ffmpeg\bin";

        private static ServiceYoutube _instance;

        public static ServiceYoutube Instance(string linkYoutube, HttpServerUtilityBase server)
        {
            _instance = new ServiceYoutube(linkYoutube, server);
            return ServiceYoutube._instance;
        }

        public ServiceYoutube(string linkYoutube, HttpServerUtilityBase server)
        {
            this.FormatType = 22;
            this.LinkYoutube = linkYoutube;
            this.FullLinkDownload = "youtube-dl -f 22 -g " + linkYoutube;
            this.Server = server;
        }

        public async Task<string> GetLinkYoutube(string token, string linkYoutube)
        {
            var id = YoutubeClient.ParseVideoId(this.LinkYoutube);

            var client = new YoutubeClient();

            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);

            var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();

            var url = streamInfo.Url;

            GetLinkLive(url, token);

            return url;
        }

        public void GetLinkLive(string url, string token)
        {
            var path = Server.MapPath(PathFfmpeg);

            var ffmpegPath = Path.Combine(path, "ffmpeg.exe");
            var ffmpegParams = @" -re -stream_loop -1 -i " + "\"" + url + "\"" + " -acodec libmp3lame  -ar 44100 -b:a 128k -pix_fmt yuv420p -profile:v baseline -s 1280x720 -bufsize 6000k -vb 400k -maxrate 1500k -deinterlace -vcodec libx264 -preset veryfast -g 30 -r 30 -f flv"
                                  + " \"" + token + "\"";

            var ffmpeg = new Process();

            ffmpeg.StartInfo.FileName = "cmd.exe";
            ffmpeg.StartInfo.WorkingDirectory = "D:\\";
            ffmpeg.StartInfo.Arguments = "/k " + ffmpegPath + " " + ffmpegParams;
            ffmpeg.Start();

            ffmpeg.WaitForExit();
            
        }
    }
}