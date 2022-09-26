using Headless.DB.Tables;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Headless.Core.xUnit.Managers.PageManager.cs
{
    public class GetPaginatedPagesForTableTests
    {
        private Core.Managers.PagesManager PagesManager { get; set; }
        private Mock<DB.HeadlessDbContext> MockedDbContext { get; set; }
        private Mock<DbSet<Page>> MockPageDbSet { get; set; }
        private Mock<DbSet<Lang>> MockLangDbSet { get; set; }
        private List<Page> MockedPages { get; set; }

        public GetPaginatedPagesForTableTests()
        {
            MockPageDbSet = new Mock<DbSet<Page>>();
            MockLangDbSet = new Mock<DbSet<Lang>>();

            MockedDbContext = new Mock<DB.HeadlessDbContext>();
            MockedDbContext.Setup(l => l.Languages).Returns(MockLangDbSet.Object);
            PagesManager = new Core.Managers.PagesManager(MockedDbContext.Object);

            Guid langID = Guid.NewGuid();
            Lang lang = new Lang
            {
                Id = langID,
                LanguageIdentifier = "de-DE",
                LanguageName = "German"
            };

            Page page1 = new Page
            {
                Id = Guid.NewGuid(),
                Title = "TestTitle1",
                Body = "<p>Test1</p>",
                Route = "/test1",
                LangId = langID
            };
            Page page2 = new Page
            {
                Id = Guid.NewGuid(),
                Title = "TestTitle2",
                Body = "<p>Test2</p>",
                Route = "/test1",
                LangId = langID
            };
            Page page3 = new Page
            {
                Id = Guid.NewGuid(),
                Title = "TestTitle3",
                Body = "<p>Test3</p>",
                Route = "/test1",
                LangId = langID
            };
            MockedPages = new List<Page>
            {
                page1,
                page2,
                page3
            };

            MockPageDbSet.As<IQueryable<Page>>().Setup(m => m.Provider).Returns(MockedPages.AsQueryable().Provider);
            MockPageDbSet.As<IQueryable<Page>>().Setup(m => m.Expression).Returns(MockedPages.AsQueryable().Expression);
            MockPageDbSet.As<IQueryable<Page>>().Setup(m => m.ElementType).Returns(MockedPages.AsQueryable().ElementType);
            MockPageDbSet.As<IQueryable<Page>>().Setup(m => m.GetEnumerator()).Returns(() => MockedPages.GetEnumerator());
            MockedDbContext.Setup(p => p.Pages).Returns(MockPageDbSet.Object);
        }

        [Fact]
        public async void GetPaginatedPagesForTables_returns_list_of_Page()
        {

            var result = await PagesManager.GetPaginatedPagesForTables(1, 1, 3);

            Assert.Equal(3, result.Count);
            Assert.Equal("TestTitle1", result[0].Title);
            Assert.Equal("TestTitle2", result[1].Title);
            Assert.Equal("TestTitle3", result[2].Title);
        }
    }
}
