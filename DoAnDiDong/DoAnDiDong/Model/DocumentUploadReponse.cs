using System;
using System.Collections.Generic;
using System.Text;

namespace DoAnDiDong.Model
{
    public class DocumentUploadReponse
    {
        public bool IsUpload { get; set; }
        public string Message { get; set; } = "";
        public List<string> DocumentUrl { get; set; } = new List<string>();
    }
}
