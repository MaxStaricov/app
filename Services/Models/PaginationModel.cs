using System.Numerics;

namespace Models;



public class PaginationModel
{
    public int Total {get; set;}
    public int Page {get; set;}
    public int PerPage {get; set;}
    public int TotalPages {get; set;}

    public PaginationModel() {}
    public PaginationModel(int total, int page, int perPage, int totalPages)
    {
        this.Total = total;
        this.Page = page;   
        this.PerPage = perPage;
        this.TotalPages = totalPages;
    }
}