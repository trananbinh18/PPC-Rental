﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPC_Rental.Models;
using System.IO;

namespace PPC_Rental.Controllers
{
    public class HomeController : Controller
    {
        K21T1_Team4Entities1 m = new K21T1_Team4Entities1();
        public ActionResult Index()
        {
            var p = m.PROPERTies.ToList();
            return View(p);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult SearchBox()
        {
            return PartialView(m);
        }

        [HttpPost]
        public ActionResult Search(string gia,string quanhuyen,string loaida, string phongtam,string phongngu,string baidauxe)
        {
            
            IEnumerable<PPC_Rental.Models.PROPERTY> ls;

            if (gia == "Dưới 50000") {
                ls = m.PROPERTies.ToList().Where(x => (x.Price < 50000 && gia != "Giá") || (x.DISTRICT.DistrictName == quanhuyen && quanhuyen != "Quận/Huyện") || (x.PROPERTY_TYPE.CodeType == loaida && loaida != "Loại Dự Án") || (x.BathRoom.ToString() == phongtam && phongtam != "Phòng tắm") || (x.BedRoom.ToString() == phongngu && phongngu != "Phòng ngủ") || (x.PackingPlace.ToString() == baidauxe && baidauxe != "Bãi đậu xe"));
            }
            else if(gia == "Từ 50000-100000")
            {
                ls = m.PROPERTies.ToList().Where(x => (x.Price >= 50000 && x.Price < 100000 && gia != "Giá") || (x.DISTRICT.DistrictName == quanhuyen && quanhuyen != "Quận/Huyện") || (x.PROPERTY_TYPE.CodeType == loaida && loaida != "Loại Dự Án") || (x.BathRoom.ToString() == phongtam && phongtam != "Phòng tắm") || (x.BedRoom.ToString() == phongngu && phongngu != "Phòng ngủ") || (x.PackingPlace.ToString() == baidauxe && baidauxe != "Bãi đậu xe"));
            }
            else if(gia == "Từ 100000-150000") {
                ls = m.PROPERTies.ToList().Where(x => (x.Price >= 100000 && x.Price < 150000 && gia != "Giá") || (x.DISTRICT.DistrictName == quanhuyen && quanhuyen != "Quận/Huyện") || (x.PROPERTY_TYPE.CodeType == loaida && loaida != "Loại Dự Án") || (x.BathRoom.ToString() == phongtam && phongtam != "Phòng tắm") || (x.BedRoom.ToString() == phongngu && phongngu != "Phòng ngủ") || (x.PackingPlace.ToString() == baidauxe && baidauxe != "Bãi đậu xe"));
            }
            else
            {
                ls = m.PROPERTies.ToList().Where(x => (x.Price >= 150000 && gia != "Giá") || (x.DISTRICT.DistrictName == quanhuyen && quanhuyen != "Quận/Huyện") || (x.PROPERTY_TYPE.CodeType == loaida && loaida != "Loại Dự Án") || (x.BathRoom.ToString() == phongtam && phongtam != "Phòng tắm") || (x.BedRoom.ToString() == phongngu && phongngu != "Phòng ngủ") || (x.PackingPlace.ToString() == baidauxe && baidauxe != "Bãi đậu xe"));
            }

            return View(ls);
        }

        public ActionResult postProject()
        {
            var model = new PROPERTY();
            return View(model);
        }

        [HttpPost]
        public ActionResult postProject(PROPERTY e, HttpPostedFileBase Avatar, List<string> chk1, List<HttpPostedFileBase> images)
        {

            //Avatar save file on webserver and sign value for model
            if(Avatar != null)
            {
                string avatar = "";
                if (Avatar.ContentLength > 0)
                {
                    var filename = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), filename);
                    Avatar.SaveAs(path);
                    avatar = filename;
                }
                e.Avatar = avatar;
            }
            
            //Image save file on webserver and add new PROPERTY_IMAGE into table PROPERTY_IMAGE
            foreach (HttpPostedFileBase img in images)
            {
                if(img != null)
                {
                    if (img.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(img.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images/"), filename);
                        img.SaveAs(path);
                        PROPERTY_IMAGE ppti = new PROPERTY_IMAGE();
                        ppti.Image = filename;
                        ppti.Property_ID = e.ID;
                        m.PROPERTY_IMAGE.Add(ppti);
                    }
                }
                else
                {
                    break;
                }
            }

            //save PROPERTY_FEATURE into PROPERTY_FEATURE table foreach Feature
                foreach (string fe in chk1)
                {
                    PROPERTY_FEATURE profe = new PROPERTY_FEATURE();
                    profe.Feature_ID = m.FEATUREs.SingleOrDefault(x => x.FeatureName == fe).ID;
                    profe.Property_ID = e.ID;
                    m.PROPERTY_FEATURE.Add(profe);
                }
            e.Created_at = DateTime.Now;
            e.Create_post = DateTime.Now;


            m.PROPERTies.Add(e);
            m.SaveChanges();


            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult requestStreets(int? District_ID) {
            return Json(
                m.STREETs.Where(x => x.DISTRICT.ID == District_ID).Select(s => new { id = s.ID, text = s.StreetName }).ToList(), JsonRequestBehavior.AllowGet); 
        }

        [HttpGet]
        public JsonResult requestWard(int? District_ID)
        {
            return Json(
                m.WARDs.Where(x => x.DISTRICT.ID == District_ID).Select(s => new { id = s.ID, text = s.WardName }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Viewlistofproject()
        {
            var viewlist = m.PROPERTies.OrderByDescending(x => x.ID).ToList();
            return View(viewlist);
        }

        public ActionResult Savedrafts()
        {
            var viewlist = m.PROPERTies.OrderByDescending(x => x.ID).ToList();
            return View(viewlist);
        }
    }
}