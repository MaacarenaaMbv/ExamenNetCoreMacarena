using ExamenNetCoreMacarena.Data;
using ExamenNetCoreMacarena.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExamenNetCoreMacarena.Repositories
{
    #region PROCEDIMIENTO
    /*
     create or alter procedure SP_GRUPO_IMAGENES_ZAPATILLAS(@POSICION int, @IDPRODUCTO int, @REGISTROS int out)
as
	select @REGISTROS = COUNT(IDIMAGEN)
	from IMAGENESZAPASPRACTICA
	where IDPRODUCTO = @IDPRODUCTO
	select IDIMAGEN, IMAGEN, IDPRODUCTO
	from
	(
		select CAST(ROW_NUMBER() OVER (ORDER BY IDPRODUCTO) AS int) AS POSICION,
		ISNULL(IDIMAGEN, 0) AS IDIMAGEN, IMAGEN, IDPRODUCTO
		FROM IMAGENESZAPASPRACTICA
		WHERE IDPRODUCTO = @IDPRODUCTO
	)
	AS QUERY
	WHERE QUERY.POSICION = @POSICION
go
     */
    #endregion
    public class RepositoryZapatilla
    {
        private ZapatillaContext context;

        public RepositoryZapatilla(ZapatillaContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillasAsync()
        {
            return await this.context.Zapatillas.ToListAsync();
        }

        public async Task<ModelPaginacionZapatillas> GetDatosPaginacionZapatillasAsync(int posicion, int idproducto)
        {
            ModelPaginacionZapatillas model = new ModelPaginacionZapatillas();
            model.Zapatilla = await this.context.Zapatillas.FirstOrDefaultAsync(p => p.IdProducto == idproducto);
            string sql = "SP_GRUPO_IMAGENES_ZAPATILLAS @POSICION, @IDPRODUCTO, @REGISTROS OUT";
            SqlParameter paramPosicion = new SqlParameter("@POSICION", posicion);
            SqlParameter paramIdProducto = new SqlParameter("@IDPRODUCTO", idproducto);
            SqlParameter paramRegistros = new SqlParameter("@REGISTROS", -1);
            paramRegistros.Direction = ParameterDirection.Output;
            var consulta = this.context.ImagenesZapatillas.FromSqlRaw(sql, paramPosicion, paramIdProducto,paramRegistros);
            List<ImagenesZapatilla> imagenes = await consulta.ToListAsync();
            model.ImagenesZapatilla = imagenes.FirstOrDefault();
            model.NumeroRegistros = (int)paramRegistros.Value;
            return model;
        }

        public async Task<int> GetMaxIdImagenZapatillas()
        {
            if (await this.context.ImagenesZapatillas.CountAsync() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.ImagenesZapatillas.MaxAsync(x => x.IdImagen) + 1;
            }
        }

        public async Task InsertarImagenes(List<string> imagenes, int idZapatilla)
        {
            foreach (string imagen in imagenes)
            {
                int id = await GetMaxIdImagenZapatillas();
                ImagenesZapatilla imagenZapa = new ImagenesZapatilla
                {
                    IdImagen = id,
                    IdProducto = idZapatilla,
                    Imagen = imagen
                };
                await this.context.ImagenesZapatillas.AddAsync(imagenZapa);
                await this.context.SaveChangesAsync();
            }
        }
    }
}
