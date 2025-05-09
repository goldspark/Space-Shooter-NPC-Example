﻿using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.BehaviorTree.AIEntities
{
    public class Gunboat : GoldTreeBase
    {
        string code = "W1RyZWVdClNlbGVjdG9yMQ0KUGFyYWxsZWw2KFBhcmVudD1TZWxlY3RvcjEpClNlcXVlbmNlMTIoUGFyZW50PVBhcmFsbGVsNikKU2VsZWN0b3IxMyhQYXJlbnQ9U2VxdWVuY2UxMikKR2V0TmVhcmJ5U2hpcHM0KFBhcmVudD1TZWxlY3RvcjEzKQpGb2xsb3dQbGF5ZXIxNChQYXJlbnQ9U2VsZWN0b3IxMykKV2FpdDMoUGFyZW50PVNlcXVlbmNlMTIpClNlcXVlbmNlNyhQYXJlbnQ9UGFyYWxsZWw2KQpJc1RhcmdldFNldDgoUGFyZW50PVNlcXVlbmNlNykKTW92ZUFyb3VuZFRhcmdldDkoUGFyZW50PVNlcXVlbmNlNykKUGFyYWxsZWwxMChQYXJlbnQ9U2VxdWVuY2U3KQpNb3ZlVG8xMShQYXJlbnQ9UGFyYWxsZWwxMCkKU2hvb3RBdEVuZW15MTAoUGFyZW50PVBhcmFsbGVsMTApCltQYXJhbGxlbDZdCgpbR2V0TmVhcmJ5U2hpcHMzM10KU3RyaW5nIGtleU5hbWUgPSAidGFyZ2V0IjsKW1dhaXQzXQpGbG9hdCB3YWl0VGltZSA9IDAuMTsKRmxvYXQgbWF4V2FpdFRpbWUgPSAxNS4wOwpGbG9hdCBtaW5XYWl0VGltZSA9IDguMDsKQm9vbCByYW5kb21pemVkID0gdHJ1ZTsKCgoKCgpbUmFuZG9tMTRdCkZsb2F0IHBlcmNlbnQgPSA1LjA7CgoKCgpbV2FpdDE1XQpGbG9hdCB3YWl0VGltZSA9IDIuMDsKRmxvYXQgbWF4V2FpdFRpbWUgPSAzOwpGbG9hdCBtaW5XYWl0VGltZSA9IDAuMTsKQm9vbCByYW5kb21pemVkID0gdHJ1ZTsKCgoKCltJc1RhcmdldFNldDhdCgpbR2V0TmVhcmJ5U2hpcHMzNV0KU3RyaW5nIGtleU5hbWUgPSAidGFyZ2V0IjsKCltNb3ZlQXJvdW5kVGFyZ2V0OV0KRmxvYXQgbWluUmFkaXVzID0gMTUwLjA7CkZsb2F0IG1heFJhZGl1cyA9IDQwMC4wOwoKCgoKW1JhbmRvbTIwXQpGbG9hdCBwZXJjZW50ID0gMTUuMDsKCgoKCgoKCltMb29rQXQyNV0KU3RyaW5nIGVudGl0eUtleSA9ICJ0YXJnZXQiOwoKCltSb2xsU2lkZXdheXMyMl0KCltNb3ZlRm9yU2Vjb25kczI2XQoKCgpbU2hvb3RBdEVuZW15MjVdCkZsb2F0IG1heEZpcmluZ1RpbWUgPSAyLjA7CkZsb2F0IG1heENvb2xkb3duVGltZSA9IDEuMDsKQm9vbCByYW5kb21pemVkID0gdHJ1ZTsKCgoKCgpbUmFuZG9tMjddCkZsb2F0IHBlcmNlbnQgPSAzNS4wOwoKCgoKCgoKCgpbTG9va0F0VGltZWQzNl0KU3RyaW5nIGVudGl0eUtleSA9ICJ0YXJnZXQiOwpGbG9hdCBtaW5Mb29rVGltZSA9IDMuMDsKRmxvYXQgbWF4TG9va1RpbWUgPSAxMC4wOwoKW01vdmVUb1RpbWVkNDRdCkJvb2wgaXNEeW5hbWljRW50aXR5ID0gdHJ1ZTsKU3RyaW5nIGVudGl0eUtleSA9ICJ0YXJnZXQiOwpGbG9hdCBtb3ZlVGltZSA9IDU7CgoKCgoKCgoKCgoKW1Nob290QXRFbmVteTMyXQpGbG9hdCBtYXhGaXJpbmdUaW1lID0gNS4wOwpGbG9hdCBtYXhDb29sZG93blRpbWUgPSAyLjA7CkJvb2wgcmFuZG9taXplZCA9IHRydWU7CgoKCgoKCgoKCgoKCgoKCgoKCgpbTW92ZVRvVGltZWQzM10KQm9vbCBpc0R5bmFtaWNFbnRpdHkgPSBmYWxzZTsKU3RyaW5nIGVudGl0eUtleSA9ICJ0YXJnZXQiOwpGbG9hdCBtb3ZlVGltZSA9IDU7CgoKCgoKCgoKCgpbU2hvb3RBdEVuZW15MTBdCkZsb2F0IG1heEZpcmluZ1RpbWUgPSAyLjA7CkZsb2F0IG1heENvb2xkb3duVGltZSA9IDEuMDsKQm9vbCByYW5kb21pemVkID0gdHJ1ZTsKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgpbU2VxdWVuY2UxMl0KCltHZXROZWFyYnlTaGlwczRdClN0cmluZyBrZXlOYW1lID0gInRhcmdldCI7CltXYWl0M10KRmxvYXQgd2FpdFRpbWUgPSAwLjE7CkZsb2F0IG1heFdhaXRUaW1lID0gMTUuMDsKRmxvYXQgbWluV2FpdFRpbWUgPSA4LjA7CkJvb2wgcmFuZG9taXplZCA9IHRydWU7CgoKCgoKW0lzVGFyZ2V0U2V0OF0KCltNb3ZlQXJvdW5kVGFyZ2V0OV0KRmxvYXQgbWluUmFkaXVzID0gMTUwLjA7CkZsb2F0IG1heFJhZGl1cyA9IDQwMC4wOwoKCgoKW01vdmVUbzExXQpCb29sIGlzRHluYW1pY0VudGl0eSA9IGZhbHNlOwpTdHJpbmcgZW50aXR5S2V5ID0gInRhcmdldCI7CltTaG9vdEF0RW5lbXkxMF0KRmxvYXQgbWF4RmlyaW5nVGltZSA9IDIuMDsKRmxvYXQgbWF4Q29vbGRvd25UaW1lID0gMS4wOwpCb29sIHJhbmRvbWl6ZWQgPSB0cnVlOwoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoK";
        public override GoldNode Start()
        {
            return AILoader.LoadBHT(this, code);
        }
    }
}
