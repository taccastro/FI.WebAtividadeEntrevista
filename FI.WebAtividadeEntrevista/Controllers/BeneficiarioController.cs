using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAtividadeEntrevista.DAO;
using WebAtividadeEntrevista.DTOs;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();


            if (!bo.ValidarCPF(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF inválido.");
            }


            if (bo.VerificarExistencia(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("Já existe um beneficiário com esse CPF para o mesmo cliente.");
            }

            try
            {
                model.Id = bo.Incluir(new Beneficiario()
                {
                    Nome = model.Nome,
                    CPF = model.CPF,
                    IDCLIENTE = model.ClienteId
                });

                return Json(new { success = true, message = "Beneficiário incluído com sucesso." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Erro ao incluir beneficiário: " + ex.Message });
            }
        }


        [HttpPost]
        public JsonResult Alterar(BeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Beneficiario()
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    CPF = model.CPF,
                    IDCLIENTE = model.ClienteId
                });

                return Json("Atualização efetuada com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoBeneficiario bo = new BoBeneficiario();
            Beneficiario beneficiario = bo.Consultar(id);
            Models.BeneficiarioModel model = null;

            if (beneficiario != null)
            {
                model = new BeneficiarioModel()
                {
                    Id = beneficiario.Id,
                    Nome = beneficiario.Nome,
                    CPF = beneficiario.CPF,
                    ClienteId = beneficiario.IDCLIENTE
                };
            }

            ViewBag.ClienteId = model?.ClienteId;  // Alterado para usar ViewBag.ClienteId
            return View(model);
        }

        [HttpPost]
        public JsonResult BeneficiarioList(int idCliente, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting?.Split(' ');

                if (array != null && array.Length > 0)
                    campo = array[0];

                if (array != null && array.Length > 1)
                    crescente = array[1];

                // Passa o idCliente para o método Pesquisa
                List<Beneficiario> beneficiarios = new BoBeneficiario().Pesquisa(
                    idCliente, jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                // Retorna os beneficiários específicos do cliente
                return Json(new { Result = "OK", Records = beneficiarios, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpDelete]
        public JsonResult Excluir(long id)
        {
            try
            {
                BoBeneficiario beneficiario = new BoBeneficiario();
                beneficiario.Excluir(id); // Executa a exclusão sem lançar exceções de sucesso

                return Json("Beneficiário excluído com sucesso.");
            }
            catch (Exception ex)
            {
                // Captura qualquer erro e retorna a mensagem
                return Json(new { success = false, message = ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}
