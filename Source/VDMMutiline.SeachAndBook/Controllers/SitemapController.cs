using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SeachAndBook.Models;
using VDMMutiline.Biz;
using VDMMutiline.Core.UI;
namespace VDMMutiline.SeachAndBook.Controllers
{
    public class SitemapController : PublishController
    {
        // GET: Sitemap
        [Route("sitemap.xml")]
        public ActionResult Index()
        {
            var sitemapNodes = GetSitemapNodes();

            string xsl = "<?xml-stylesheet type=\"text/xsl\" href=\"/xml-sitemap.xsl\" ?> ";
            string xml = "<?xml version='1.0' encoding='UTF-8\' ?>" + xsl + GetSitemapDocument(sitemapNodes);
            return this.Content(xml, "application/xml", Encoding.UTF8);
        }

        public List<SysCategoryInfo> GetCate()
        {
            var param = new SysCategoryParam()
            {
                SysCategoryFilter = new SysCategoryFilter
                {
                    IsMenu = true,
                    type = Constant.CateType.NoiDung,
                    ListUserName = VDMMutiline.Core.UI.CommonUI.GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
                },
            };
            var biz = new SysCategoryBiz();
            biz.Search(param);
            return param.SysCategoryInfos.ToList();
        }
        public IReadOnlyCollection<SitemapNode> GetSitemapNodes()
        {
            var baseUri = "http://" + Request.Url.Host;
            List<SitemapNode> nodes = new List<SitemapNode>();
            nodes.Add(
                new SitemapNode()
                {
                    Url = baseUri,
                    Priority = 1
                });
            nodes.Add(
               new SitemapNode()
               {

                   Url = baseUri,
                   Priority = 0.9
               });
            var listcate = GetCate();
            foreach (var item in listcate.Where(z => z.ParentId == null).OrderBy(z => z.Thutu))
            {
                if (item.IsContend != true)
                {
                    if (!string.IsNullOrEmpty(item.UrlConten) && item.UrlConten!="#")
                    {
                        nodes.Add(new SitemapNode()
                        {
                            Url = baseUri + "/" + item.UrlConten,
                            Priority = 0.9
                        });
                    }
                    else
                    {
                        nodes.Add(new SitemapNode()
                        {
                            Url = baseUri + "/" + Url.Action("Index", "FontEndnew", new { ChuyenMuc = item.SEOUrl }),
                            Priority = 0.9
                        });
                    }
                }
                else
                {
                    var objs = GetinfoArtical(item.IdContend);
                    var url = "#";
                    if (objs != null)
                    {
                        url = Url.Action("ChiTiet", "FontEndnew", new { key = objs.SEOUrl });
                    }
                    if (url != "#")
                    {
                        nodes.Add(new SitemapNode()
                        {
                            Url = baseUri + "/1" + url,
                            Priority = 0.9
                        });
                    }
                }

            }


            nodes.AddRange(getlltintuc().Select(item => new SitemapNode()
            {
                Url = baseUri + "/" + Url.Action("ChiTiet", "FontEndnew", new { key = item.SEOUrl }),
                Frequency = SitemapFrequency.Weekly,
                Priority = 0.8
            }));
            return nodes;
        }
        private TblArticleInfo GetinfoArtical(int? id)
        {
            var param = new TblArticleParam { Id = id ?? 0 };
            var biz = new TblArticleBiz();
            biz.SetupDisplayForm(param);
            return param.TblArticleInfo;
        }
        private List<TblArticleInfo> getlltintuc()
        {
            var list = new List<TblArticleInfo>();
            var bizTinTuc = new TblArticleBiz();
            var filter = new TblArticleFilter()
            {
                typeid = Constant.CateType.NoiDung,
                ListUserName = GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
            };
            var paramTinTuc = new TblArticleParam { };
            paramTinTuc.TblArticleFilter = filter;
            bizTinTuc.SearchHome(paramTinTuc);
            return paramTinTuc.TblArticleInfos;
        }
        public string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");
            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                    sitemapNode.LastModified == null ? null : new XElement(
                        xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    sitemapNode.Frequency == null ? null : new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.Priority == null ? null : new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }
            XDocument document = new XDocument(root);
            return document.ToString();
        }
    }
}