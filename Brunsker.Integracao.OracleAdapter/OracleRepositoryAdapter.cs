using Brunsker.Integracao.Domain.Adapters;
using Brunsker.Integracao.Domain.Models;
using Dapper;
using Dapper.Oracle;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace Brunsker.Integracao.OracleAdapter
{
    public class OracleRepositoryAdapter : IOracleRepositoryAdapter
    {
        private readonly DbConnectionDbRepositoryAdapter _config;
        private readonly ILogger _logger;
        public OracleRepositoryAdapter(DbConnectionDbRepositoryAdapter config, ILoggerFactory loggerFactory)
        {
            _config = config ?? throw new ArgumentNullException(nameof(_config));

            _logger = loggerFactory?.CreateLogger<OracleRepositoryAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /*NOVO*/
        public async Task<IEnumerable<ConsultaCliente>> SelectConsultaCliente()
        {
            IEnumerable<ConsultaCliente> consultas = null;

            try
            {
                string sql = "pkg_clientes_nfe.PROC_SEL_CLIENTES_ALTERADOS";

                using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

                if (conn.State == ConnectionState.Closed) conn.Open();

                var parameters = new OracleDynamicParameters();

                parameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                consultas = await conn.QueryAsync<ConsultaCliente>(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return consultas;
        }
        public async Task<IEnumerable<PreEntrada>> SelectPreEntrada()
        {
            IEnumerable<PreEntrada> preEntradas = null;

            try
            {
                string sql = "pkg_pre_entrada.PESQ_PCMOVPREENT";

                using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

                if (conn.State == ConnectionState.Closed) conn.Open();

                var parameters = new OracleDynamicParameters();

                parameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                preEntradas = await conn.QueryAsync<PreEntrada>(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return preEntradas;
        }

        public async Task<IEnumerable<PreLancamento>> SelectLancamento()
        {
            IEnumerable<PreLancamento> lancamentos = null;

            try
            {
                string sql = "pkg_pre_entrada.PESQ_PCLANCPREENT";

                using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

                if (conn.State == ConnectionState.Closed) conn.Open();

                var parameters = new OracleDynamicParameters();

                parameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                lancamentos = await conn.QueryAsync<PreLancamento>(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return lancamentos;
        }
        public async Task ConfirmaEnvioDadosApi(int seqCliente, string rowId, string sql)
        {
            try
            {
                using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

                if (conn.State == ConnectionState.Closed) conn.Open();

                var parameters = new OracleDynamicParameters();

                parameters.Add("pSEQ_CLIENTE", seqCliente);
                parameters.Add("SINC_ROWID", rowId);

                await conn.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task ConfirmaEnvioDadosApi(ConsultaCliente consulta, string sql)
        {
            try
            {
                using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

                if (conn.State == ConnectionState.Closed) conn.Open();

                var parameters = new OracleDynamicParameters();

                parameters.Add("pCODCLI", consulta.CODCLI);
                parameters.Add("pSEQ_CLIENTE", consulta.SEQ_CLIENTE);

                await conn.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task<List<Message>> ClientesAsync(List<Message> clientes, string package)
        {

            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            foreach (var item in clientes)
            {
                try
                {
                    var cli = JsonConvert.DeserializeObject<Cliente>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pCODCLI", cli.CODCLI);
                    dynamicParameters.Add("pCLIENTE", cli.CLIENTE);
                    dynamicParameters.Add("pESTENT", cli.ESTENT);
                    dynamicParameters.Add("pCGCENT", cli.CGCENT);
                    dynamicParameters.Add("pIEENT", cli.IEENT);
                    dynamicParameters.Add("pBLOQUEIO", cli.BLOQUEIO);
                    dynamicParameters.Add("pBLOQUEIOSEFAZ", cli.BLOQUEIOSEFAZ);
                    dynamicParameters.Add("pBLOQUEIOSEFAZPED", cli.BLOQUEIOSEFAZPED);
                    dynamicParameters.Add("pBLOQUEIODEFINITIVO", cli.BLOQUEIODEFINITIVO);
                    dynamicParameters.Add("pBLOQUEIOINATIVIDADE", cli.BLOQUEIOINATIVIDADE);
                    dynamicParameters.Add("pSEQ_CLIENTE", cli.SEQ_CLIENTE);
                    dynamicParameters.Add("pSTRING_BANCO", cli.STRING_BANCO);


                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    clientes.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    clientes.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return clientes;
        }
        public async Task<List<Message>> PCESTAsync(List<Message> pcests, string package)
        {

            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            foreach (var item in pcests)
            {
                try
                {
                    var pcest = JsonConvert.DeserializeObject<PCEST>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pCODFILIAL", pcest.CODFILIAL);
                    dynamicParameters.Add("pCODPROD", pcest.CODPROD);
                    dynamicParameters.Add("pSEQ_CLIENTE", pcest.SEQ_CLIENTE);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    pcests.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    pcests.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return pcests;
        }
        public async Task<List<Message>> PCPRODFILIALAsync(List<Message> pcprodfiliais, string package)
        {

            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            foreach (var item in pcprodfiliais)
            {
                try
                {
                    var pcprodfillial = JsonConvert.DeserializeObject<PCPRODFILIAL>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pCODFILIAL", pcprodfillial.CODFILIAL);
                    dynamicParameters.Add("pCODPROD", pcprodfillial.CODPROD);
                    dynamicParameters.Add("pPROIBIDAVENDA", pcprodfillial.PROIBIDAVENDA);
                    dynamicParameters.Add("pFORALINHA", pcprodfillial.FORALINHA);
                    dynamicParameters.Add("pORIGMERCTRIB", pcprodfillial.ORIGMERCTRIB);
                    dynamicParameters.Add("pSEQ_CLIENTE", pcprodfillial.SEQ_CLIENTE);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    pcprodfiliais.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    pcprodfiliais.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return pcprodfiliais;
        }
        public async Task<List<Message>> PCTABPRAsync(List<Message> pctabprs, string package)
        {

            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in pctabprs)
            {
                try
                {
                    var pctabpr = JsonConvert.DeserializeObject<PCTABPR>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pCODPROD", pctabpr.CODPROD);
                    dynamicParameters.Add("pNUMREGIAO", pctabpr.NUMREGIAO);
                    dynamicParameters.Add("pCODST", pctabpr.CODST);
                    dynamicParameters.Add("pSEQ_CLIENTE", pctabpr.SEQ_CLIENTE);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    pctabprs.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    pctabprs.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return pctabprs;
        }
        public async Task<List<Message>> PCNEGFORNECAsync(List<Message> pcnegfornecs, string package)
        {

            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            foreach (var item in pcnegfornecs)
            {
                try
                {
                    var pcnegfornec = JsonConvert.DeserializeObject<PCNEGFORNEC>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pCODFILIAL", pcnegfornec.CODFILIAL);
                    dynamicParameters.Add("pCODPROD", pcnegfornec.CODPROD);
                    dynamicParameters.Add("pCODFORNEC", pcnegfornec.CODFORNEC);
                    dynamicParameters.Add("pSEQ_CLIENTE", pcnegfornec.SEQ_CLIENTE);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    pcnegfornecs.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    pcnegfornecs.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return pcnegfornecs;
        }
        public async Task<List<Message>> PCNFBASEENTAsync(List<Message> pcnfbaseents, string package)
        {

            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            foreach (var item in pcnfbaseents)
            {
                try
                {
                    var pcnfbaseent = JsonConvert.DeserializeObject<PCNFBASEENT>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", pcnfbaseent.SEQ_CLIENTE);
                    dynamicParameters.Add("pCODFILIAL", pcnfbaseent.CODFILIALNF);
                    dynamicParameters.Add("pNUMTRANSENT", pcnfbaseent.NUMTRANSENT);
                    dynamicParameters.Add("pDTENTRADA", pcnfbaseent.DTENTRADA);
                    dynamicParameters.Add("pDTEMISSAO", pcnfbaseent.DTEMISSAO);
                    dynamicParameters.Add("pESPECIE", pcnfbaseent.ESPECIE);
                    dynamicParameters.Add("pSERIE", pcnfbaseent.SERIE);
                    dynamicParameters.Add("pNUMNOTA", pcnfbaseent.NUMNOTA);
                    dynamicParameters.Add("pCODFORNEC", pcnfbaseent.CODFORNEC);
                    dynamicParameters.Add("pUF", pcnfbaseent.UF);
                    dynamicParameters.Add("pVLTOTAL", pcnfbaseent.VLTOTAL);
                    dynamicParameters.Add("pCODCONT", pcnfbaseent.CODCONT);
                    dynamicParameters.Add("pCODFISCAL", pcnfbaseent.CODFISCAL);
                    dynamicParameters.Add("pOBS", pcnfbaseent.OBS);
                    dynamicParameters.Add("pSTRING_BANCO", pcnfbaseent.STRING_BANCO);


                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    pcnfbaseents.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    pcnfbaseents.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return pcnfbaseents;
        }
        public async Task<List<Message>> PCTABTRIBENTAsync(List<Message> pctabtribents, string package)
        {

            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in pctabtribents)
            {
                try
                {
                    var pctabtribent = JsonConvert.DeserializeObject<PCTABTRIBENT>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pCODPROD", pctabtribent.CODPROD);
                    dynamicParameters.Add("pUFORIGEM", pctabtribent.UFORIGEM);
                    dynamicParameters.Add("pUFDESTINO", pctabtribent.UFDESTINO);
                    dynamicParameters.Add("pSEQ_CLIENTE", pctabtribent.SEQ_CLIENTE);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    pctabtribents.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    pctabtribents.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return pctabtribents;
        }
        public async Task<List<Message>> PCTRIBENTRADAAsync(List<Message> pctribentradas, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in pctribentradas)
            {
                try
                {
                    var pctribentrada = JsonConvert.DeserializeObject<PCTRIBENTRADA>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pNCM", pctribentrada.NCM);
                    dynamicParameters.Add("pCODFILIAL", pctribentrada.CODFILIAL);
                    dynamicParameters.Add("pUFORIGEM", pctribentrada.UFORIGEM);
                    dynamicParameters.Add("pTIPOFORNEC", pctribentrada.TIPOFORNEC);
                    dynamicParameters.Add("pSEQ_CLIENTE", pctribentrada.SEQ_CLIENTE);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    pctribentradas.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    pctribentradas.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return pctribentradas;
        }
        public async Task<List<Message>> PCCFOAsync(List<Message> pccfos, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in pccfos)
            {

                try
                {
                    var pccfo = JsonConvert.DeserializeObject<PCCFO>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", pccfo.SEQ_CLIENTE);
                    dynamicParameters.Add("pCODFISCAL", pccfo.CODFISCAL);
                    dynamicParameters.Add("pDESCCFO", pccfo.DESCCFO);
                    dynamicParameters.Add("pCODOPER", pccfo.CODOPER);
                    dynamicParameters.Add("pSTRING_BANCO", pccfo.STRING_BANCO);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    pccfos.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    pccfos.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return pccfos;
        }
        public async Task<List<Message>> PCCODFABRICA(List<Message> mensagens, string package)
        {
            using (var conn = new OracleConnection(_config.Connection.ConnectionString))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                foreach (var item in mensagens)
                {
                    try
                    {
                        var pccodfabrica = JsonConvert.DeserializeObject<PCCODFABRICA>(item.Content);

                        OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                        dynamicParameters.Add("pROWID", pccodfabrica.ROWID_TB);
                        dynamicParameters.Add("pSEQ_CLIENTE", pccodfabrica.SEQ_CLIENTE);
                        dynamicParameters.Add("pCODPROD", pccodfabrica.CODPROD);
                        dynamicParameters.Add("pCODFORNEC", pccodfabrica.CODFORNEC);
                        dynamicParameters.Add("pCODFAB", pccodfabrica.CODFAB);
                        dynamicParameters.Add("pFATOR", pccodfabrica.FATOR);
                        dynamicParameters.Add("pSTRING_BANCO", pccodfabrica.STRING_BANCO);
                        dynamicParameters.Add("pDTINSERT", pccodfabrica.DT_INSERT);

                        await conn.ExecuteAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                    }
                }
            }

            return mensagens;
        }
        public async Task<List<Message>> PCPARCELASC(List<Message> mensagens, string package)
        {
            using (var conn = new OracleConnection(_config.Connection.ConnectionString))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                foreach (var item in mensagens)
                {
                    try
                    {
                        var pcparcelasc = JsonConvert.DeserializeObject<PCPARCELASC>(item.Content);

                        OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                        dynamicParameters.Add("pROWID", pcparcelasc.ROWID_TB);
                        dynamicParameters.Add("pSEQ_CLIENTE", pcparcelasc.SEQ_CLIENTE);
                        dynamicParameters.Add("pCODPARCELA", pcparcelasc.CODPARCELA);
                        dynamicParameters.Add("pDESCRICAO", pcparcelasc.DESCRICAO);
                        dynamicParameters.Add("pQTDMAXPARCELA", pcparcelasc.QTDMAXPARCELA);
                        dynamicParameters.Add("pSTRING_BANCO", pcparcelasc.STRING_BANCO);
                        dynamicParameters.Add("pDTINSERT", pcparcelasc.DT_INSERT);

                        await conn.ExecuteAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                    }
                }
            }

            return mensagens;
        }
        public async Task<List<Message>> PREENTRADA_WINTHOR(List<Message> mensagens, string package)
        {
            using (var conn = new OracleConnection(_config.Connection.ConnectionString))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                foreach (var item in mensagens)
                {
                    try
                    {
                        var preEntrada = JsonConvert.DeserializeObject<NotaFiscalPreEntrada>(item.Content);

                        OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                        dynamicParameters.Add("pSEQ_CLIENTE", preEntrada.SEQ_CLIENTE);
                        dynamicParameters.Add("pSTRING_BANCO", preEntrada.STRING_BANCO);
                        dynamicParameters.Add("pDT_INSERT", preEntrada.DT_INSERT);
                        dynamicParameters.Add("pNUMNOTA", preEntrada.NUMNOTA);
                        dynamicParameters.Add("pESPECIE", preEntrada.ESPECIE);
                        dynamicParameters.Add("pDTEMISSAO", preEntrada.DTEMISSAO);
                        dynamicParameters.Add("pCODFORNEC", preEntrada.CODFORNEC);
                        dynamicParameters.Add("pCGC", preEntrada.CGC);
                        dynamicParameters.Add("pCHAVENFE", preEntrada.CHAVENFE);
                        dynamicParameters.Add("pCODDEVOL", preEntrada.CODDEVOL);
                        dynamicParameters.Add("pNUMTRANSENT", preEntrada.NUMTRANSENT);
                        dynamicParameters.Add("pROWID_TB", preEntrada.ROWID_TB);


                        await conn.ExecuteAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                    }
                }
            }

            return mensagens;
        }
        public async Task<List<Message>> PCCONSUM(List<Message> mensagens, string package)
        {
            using (var conn = new OracleConnection(_config.Connection.ConnectionString))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                foreach (var item in mensagens)
                {
                    try
                    {
                        var pcconsum = JsonConvert.DeserializeObject<PCCONSUM>(item.Content);

                        OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                        dynamicParameters.Add("pSEQ_CLIENTE", pcconsum.SEQ_CLIENTE);
                        dynamicParameters.Add("pNUMREGIAOPADRAO", pcconsum.NUMREGIAOPADRAO);
                        dynamicParameters.Add("pUSANEGFORNEC", pcconsum.USANEGFORNEC);
                        dynamicParameters.Add("pUSATRIBENTPORUF", pcconsum.USATRIBENTPORUF);

                        await conn.ExecuteAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                    }
                }
            }

            return mensagens;
        }
        public async Task<List<Message>> PCESTCOM(List<Message> mensagens, string package)
        {
            using (var conn = new OracleConnection(_config.Connection.ConnectionString))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                foreach (var item in mensagens)
                {
                    try
                    {
                        var pcestcom = JsonConvert.DeserializeObject<PCESTCOM>(item.Content);

                        OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                        dynamicParameters.Add("pSEQ_CLIENTE", pcestcom.SEQ_CLIENTE);
                        dynamicParameters.Add("pNUMTRANSENT", pcestcom.NUMTRANSENT);
                        dynamicParameters.Add("pNUMTRANSVENDA", pcestcom.NUMTRANSVENDA);
                        dynamicParameters.Add("pVLDEVOLUCAO", pcestcom.VLDEVOLUCAO);

                        await conn.ExecuteAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);

                        mensagens.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                    }
                }
            }

            return mensagens;
        }

        /*END NOVO*/

        public async Task<List<Message>> DepartamentoAsync(List<Message> departamentos, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in departamentos)
            {
                try
                {
                    var dep = JsonConvert.DeserializeObject<Departamento>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pROWID", dep.ROWID_TB);
                    dynamicParameters.Add("pSEQ_CLIENTE", dep.SEQ_CLIENTE);
                    dynamicParameters.Add("pCODEPTO", dep.CODEPTO);
                    dynamicParameters.Add("pDESCRICAO", dep.DESCRICAO);
                    dynamicParameters.Add("pCODFORNEC", dep.CODFORNEC);
                    dynamicParameters.Add("pFORNECEDOR", dep.FORNECEDOR);
                    dynamicParameters.Add("pSTRING_BANCO", dep.STRING_BANCO);
                    dynamicParameters.Add("pDTINSERT", dep.DT_INSERT);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    departamentos.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    departamentos.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return departamentos;
        }
        public async Task<List<Message>> FilialAsync(List<Message> filiais, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in filiais)
            {
                try
                {
                    var fil = JsonConvert.DeserializeObject<Filial>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pROWID", fil.ROWID_TB);
                    dynamicParameters.Add("pSEQ_CLIENTE", fil.VSEQ_CLIENTE);
                    dynamicParameters.Add("pCODIGO", fil.CODIGO);
                    dynamicParameters.Add("pCGC", fil.CGC);
                    dynamicParameters.Add("pRAZAOSOCIAL", fil.RAZAOSOCIAL);
                    dynamicParameters.Add("pNUMREGIAOPADRAO", fil.NUMREGIAOPADRAO);
                    dynamicParameters.Add("pESTADO", fil.ESTADO);
                    dynamicParameters.Add("pSTRING_BANCO", fil.VSTRING_BANCO);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    filiais.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    filiais.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }
            return filiais;
        }
        public async Task<List<Message>> ItemAsync(List<Message> itensJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in itensJson)
            {
                try
                {
                    var i = JsonConvert.DeserializeObject<Item>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pROWID", i.ROWID_TB);
                    dynamicParameters.Add("pSEQ_CLIENTE", i.SEQ_CLIENTE);
                    dynamicParameters.Add("pNUMPED", i.NUMPED);
                    dynamicParameters.Add("pCODPROD", i.CODPROD);
                    dynamicParameters.Add("pMOEDA", i.MOEDA);
                    dynamicParameters.Add("pDTULTCOMPRA", i.DTULTCOMPRA);
                    dynamicParameters.Add("pDTULTENT", i.DTULTENT);
                    dynamicParameters.Add("pPERCICMRED", i.PERCICMRED);
                    dynamicParameters.Add("pPERICM", i.PERICM);
                    dynamicParameters.Add("pQTENTREGUE", i.QTENTREGUE);
                    dynamicParameters.Add("pQTPEDIDA", i.QTPEDIDA);
                    dynamicParameters.Add("pPLIQUIDO", i.PLIQUIDO);
                    dynamicParameters.Add("pPCOMPRA", i.PCOMPRA);
                    dynamicParameters.Add("pNUMSEQ", i.NUMSEQ);
                    dynamicParameters.Add("pPERIPI", i.PERIPI);
                    dynamicParameters.Add("pPERCIVA", i.PERCIVA);
                    dynamicParameters.Add("pPERCALIQINT", i.PERCALIQINT);
                    dynamicParameters.Add("pPERCALIQEXT", i.PERCALIQEXT);
                    dynamicParameters.Add("pPERCALIQEXTGUIA", i.PERCALIQEXTGUIA);
                    dynamicParameters.Add("pPERCREDICMS", i.PERCREDICMS);
                    dynamicParameters.Add("pPERPIS", i.PERCREDICMS);
                    dynamicParameters.Add("pPERCOFINS", i.PERCREDICMS);
                    dynamicParameters.Add("pSTRING_BANCO", i.STRING_BANCO);
                    dynamicParameters.Add("pDTINSERT", i.DT_INSERT);
                    dynamicParameters.Add("pMULTIPLOCOMPRAS", i.MULTIPLOCOMPRAS);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    itensJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    itensJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return itensJson;
        }
        public async Task<List<Message>> LancamentoAsync(List<Message> lancamentosJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in lancamentosJson)
            {
                try
                {
                    var lan = JsonConvert.DeserializeObject<Lancamento>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", lan.SEQ_CLIENTE);
                    dynamicParameters.Add("pRECNUM", lan.RECNUM);
                    dynamicParameters.Add("pDTLANC", lan.DTLANC);
                    dynamicParameters.Add("pCGC", lan.CGC);
                    dynamicParameters.Add("pHISTORICO", lan.HISTORICO);
                    dynamicParameters.Add("pHISTORICO2", lan.HISTORICO);
                    dynamicParameters.Add("pNUMNOTA", lan.NUMNOTA);
                    dynamicParameters.Add("pDUPLIC", lan.DUPLIC);
                    dynamicParameters.Add("pVALOR", lan.VALOR);
                    dynamicParameters.Add("pDTVENC", lan.DTVENC);
                    dynamicParameters.Add("pVPAGO", lan.VPAGO);
                    dynamicParameters.Add("pDTPAGTO", lan.DTPAGTO);
                    dynamicParameters.Add("pCODFORNEC", lan.CODFORNEC);
                    dynamicParameters.Add("pSTRING_BANCO", lan.STRING_BANCO);


                    if (package != "pkg_webserv_update_bsnotas.PROC_UPD_PCLANC")
                    {
                        dynamicParameters.Add("pROWID", lan.ROWID_TB);
                        dynamicParameters.Add("pDTINSERT", lan.DT_INSERT);
                    }

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    lancamentosJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    lancamentosJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return lancamentosJson;
        }
        public async Task<List<Message>> MovimentacaoAsync(List<Message> movimentacoesJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in movimentacoesJson)
            {
                try
                {
                    var mov = JsonConvert.DeserializeObject<Movimentacao>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pROWID", mov.ROWID_TB);
                    dynamicParameters.Add("pSEQ_CLIENTE", mov.SEQ_CLIENTE);
                    dynamicParameters.Add("pNUMPED", mov.NUMPED);
                    dynamicParameters.Add("pNUMNOTA", mov.NUMNOTA);
                    dynamicParameters.Add("pCODFORNEC", mov.CODFORNEC);
                    dynamicParameters.Add("pCODFILIAL", mov.CODFILIAL);
                    dynamicParameters.Add("pSTRING_BANCO", mov.STRING_BANCO);
                    dynamicParameters.Add("pQT", mov.QT);
                    dynamicParameters.Add("pCODPROD", mov.CODPROC);
                    dynamicParameters.Add("pCHAVENFE", mov.CHAVENFE);
                    dynamicParameters.Add("pCODAUXILIAR", mov.CODAUXILIAR);
                    dynamicParameters.Add("pDTINSERT", mov.DT_INSERT);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    movimentacoesJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    movimentacoesJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }
            return movimentacoesJson;
        }
        public async Task<List<Message>> NotaFiscalEntradaAsync(List<Message> notasFiscaisEntradaJson, string package)
        {


            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in notasFiscaisEntradaJson)
            {
                try
                {
                    var nf = JsonConvert.DeserializeObject<NotaFiscalEntrada>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pNUMNOTA", nf.NUMNOTA);
                    dynamicParameters.Add("pSERIE", nf.SERIE);
                    dynamicParameters.Add("pESPECIE", nf.ESPECIE);
                    dynamicParameters.Add("pDTEMISSAO", nf.DTEMISSAO);
                    dynamicParameters.Add("pCODFORNEC", nf.CODFORNEC);
                    dynamicParameters.Add("pFORNECEDOR", nf.FORNECEDOR);
                    dynamicParameters.Add("pCGC", nf.CGC);
                    dynamicParameters.Add("pCGCFILIAL", nf.CGCFILIAL);
                    dynamicParameters.Add("pSEQ_CLIENTE", nf.SEQ_CLIENTE);
                    dynamicParameters.Add("pCHAVENFE", nf.CHAVENFE);
                    dynamicParameters.Add("pCODDEVOL", nf.CODDEVOL);
                    dynamicParameters.Add("pNUMTRANSENT", nf.NUMTRANSENT);
                    dynamicParameters.Add("pTIPODESCARGA", nf.TIPODESCARGA);
                    dynamicParameters.Add("pDTCANCEL", nf.DTCANCEL);
                    dynamicParameters.Add("pROWID", nf.ROWID_TB);
                    dynamicParameters.Add("pSTRING_BANCO", nf.STRING_BANCO);
                    dynamicParameters.Add("pDTENT", nf.DTENT);


                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    notasFiscaisEntradaJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    notasFiscaisEntradaJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }
            return notasFiscaisEntradaJson;
        }
        public async Task<List<Message>> NotaFiscalPreEntradaAsync(List<Message> notasFiscaisPreEntradaJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in notasFiscaisPreEntradaJson)
            {
                try
                {
                    var nfePre = JsonConvert.DeserializeObject<NotaFiscalPreEntrada>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", nfePre.SEQ_CLIENTE);
                    dynamicParameters.Add("pCHAVENFE", nfePre.CHAVENFE);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    notasFiscaisPreEntradaJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    notasFiscaisPreEntradaJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }
            return notasFiscaisPreEntradaJson;
        }
        public async Task<List<Message>> NotaFiscalSaidaAsync(List<Message> notasFiscaisSaidaJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in notasFiscaisSaidaJson)
            {
                try
                {
                    var nfs = JsonConvert.DeserializeObject<NotaFiscalSaida>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", nfs.SEQ_CLIENTE);
                    dynamicParameters.Add("pNUMNOTA", nfs.NUMNOTA);
                    dynamicParameters.Add("pSERIE", nfs.SERIE);
                    dynamicParameters.Add("pDTSAIDA", nfs.DTSAIDA);
                    dynamicParameters.Add("pVLTOTAL", nfs.VLTOTAL);
                    dynamicParameters.Add("pVLBONIFIC", nfs.VLBONIFIC);
                    dynamicParameters.Add("pCODCLI", nfs.CODCLI);
                    dynamicParameters.Add("pNUMTRANSVENDA", nfs.NUMTRANSVENDA);
                    dynamicParameters.Add("pCHAVENFE", nfs.CHAVENFE);
                    dynamicParameters.Add("pSITUACAONFE", nfs.SITUACAONFE);
                    dynamicParameters.Add("pTIPOVENDA", nfs.TIPOVENDA);
                    dynamicParameters.Add("pCONDVENDA", nfs.CONDVENDA);
                    dynamicParameters.Add("pCODFISCAL", nfs.CODFISCAL);
                    dynamicParameters.Add("pCGCENT", nfs.CGCENT);
                    dynamicParameters.Add("pCGCFILIAL", nfs.CGCFILIAL);
                    dynamicParameters.Add("pIEENT", nfs.IEENT);
                    dynamicParameters.Add("pDTCANCEL", nfs.DTCANCEL);
                    dynamicParameters.Add("pSTRING_BANCO", nfs.STRING_BANCO);


                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    notasFiscaisSaidaJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    notasFiscaisSaidaJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }
            return notasFiscaisSaidaJson;
        }
        public async Task<List<Message>> PedidoAsync(List<Message> pedidosJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in pedidosJson)
            {
                try
                {
                    var ped = JsonConvert.DeserializeObject<Pedido>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pROWID", ped.ROWID_TB);
                    dynamicParameters.Add("pNUMPED", ped.NUMPED);
                    dynamicParameters.Add("pSEQ_CLIENTE", ped.SEQ_CLIENTE);
                    dynamicParameters.Add("pCGC", ped.CGC);
                    dynamicParameters.Add("pFORNECEDOR", ped.FORNECEDOR);
                    dynamicParameters.Add("pCODFORNEC", ped.CODFORNEC);
                    dynamicParameters.Add("pCODFILIAL", ped.CODFILIAL);
                    dynamicParameters.Add("pCODPARCELA", ped.CODPARCELA);
                    dynamicParameters.Add("pDTEMISSAO", ped.DATAEMISSAO);
                    dynamicParameters.Add("pVLTOTAL", ped.VLTOTAL);
                    dynamicParameters.Add("pDTFATUR", ped.DTFATUR);
                    dynamicParameters.Add("pSTRING_BANCO", ped.STRING_BANCO);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    pedidosJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    pedidosJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return pedidosJson;
        }
        public async Task<List<Message>> PCCONTA(List<Message> contas, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in contas)
            {
                try
                {
                    var conta = JsonConvert.DeserializeObject<PCCONTAS>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pROWID", conta.ROWID_TB);
                    dynamicParameters.Add("pSEQ_CLIENTE", conta.SEQ_CLIENTE);
                    dynamicParameters.Add("pCODCONTA", conta.CODCONTA);
                    dynamicParameters.Add("pCONTA", conta.CONTA);
                    dynamicParameters.Add("pCONTACONTABIL", conta.CONTACONTABIL);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    contas.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    contas.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return contas;
        }
        public async Task<List<Message>> PrestAsync(List<Message> prestJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in prestJson)
            {
                try
                {
                    var p = JsonConvert.DeserializeObject<Prest>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pROWID", p.ROWID_TB);
                    dynamicParameters.Add("pSEQ_CLIENTE", p.SEQ_CLIENTE);
                    dynamicParameters.Add("pCODCLI", p.CODCLI);
                    dynamicParameters.Add("pPREST", p.PREST);
                    dynamicParameters.Add("pDUPLIC", p.DUPLIC);
                    dynamicParameters.Add("pVALOR", p.VALOR);
                    dynamicParameters.Add("pDTVENC", p.DTVENC);
                    dynamicParameters.Add("pCODCOB", p.CODCOB);
                    dynamicParameters.Add("pVPAGO", p.VPAGO);
                    dynamicParameters.Add("pDTPAG", p.DTPAG);
                    dynamicParameters.Add("pDTEMISSAO", p.DTEMISSAO);
                    dynamicParameters.Add("pCODFILIAL", p.CODFILIAL);
                    dynamicParameters.Add("pVALORDESC", p.VALORDESC);
                    dynamicParameters.Add("pDTBAIXA", p.DTBAIXA);
                    dynamicParameters.Add("pDTDESD", p.DTDESD);
                    dynamicParameters.Add("pDTFECHA", p.DTFECHA);
                    dynamicParameters.Add("pNUMTRANSVENDA", p.NUMTRANSVENDA);
                    dynamicParameters.Add("pNUMPED", p.NUMPED);
                    dynamicParameters.Add("pSTRING_BANCO", p.STRING_BANCO);
                    dynamicParameters.Add("pDTINSERT", p.DTINSERT);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);


                    prestJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    prestJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }
            return prestJson;
        }
        public async Task<List<Message>> ProdutoAsync(List<Message> produtosJson, string package)
        {

            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in produtosJson)
            {
                try
                {
                    var prod = JsonConvert.DeserializeObject<Produto>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pROWID", prod.ROWID_TB);
                    dynamicParameters.Add("pSEQ_CLIENTE", prod.SEQ_CLIENTE);
                    dynamicParameters.Add("pCODPROD", prod.CODPROD);
                    dynamicParameters.Add("pDESCRICAO", prod.DESCRICAO);
                    dynamicParameters.Add("pEMBALAGEM", prod.EMBALAGEM);
                    dynamicParameters.Add("pUNIDADE", prod.UNIDADE);
                    dynamicParameters.Add("pCODFORNEC", prod.CODFORNEC);
                    dynamicParameters.Add("pPERICM", prod.PERICM);
                    dynamicParameters.Add("pPERCST", prod.PERCST);
                    dynamicParameters.Add("pCODAUXILIAR", prod.CODAUXILIAR);
                    dynamicParameters.Add("pCODAUXILIAR2", prod.CODAUXILIAR2);
                    dynamicParameters.Add("pCODFAB", prod.CODFAB);
                    dynamicParameters.Add("pNBM", prod.NBM);
                    dynamicParameters.Add("pDTEXCLUSAO", prod.DTEXCLUSAO);
                    dynamicParameters.Add("pCODFILIAL", prod.CODFILIAL);
                    dynamicParameters.Add("pOBS", prod.OBS);
                    dynamicParameters.Add("pOBS2", prod.OBS2);
                    dynamicParameters.Add("pCODNCMEX", prod.CODNCMEX);
                    dynamicParameters.Add("pPRECIFICESTRANGEIRA", prod.PRECIFICESTRANGEIRA);
                    dynamicParameters.Add("pTIPOMERC", prod.TIPOMERC);
                    dynamicParameters.Add("pPERCIPI", prod.PERCIPI);
                    dynamicParameters.Add("pPERCALIQINT", prod.PERCALIQINT);
                    dynamicParameters.Add("pPERCALIQEXT", prod.PERCALIQEXT);
                    dynamicParameters.Add("pPERCIVA", prod.PERCIVA);
                    dynamicParameters.Add("pCODFISCAL", prod.CODFISCAL);
                    dynamicParameters.Add("pSTRING_BANCO", prod.STRING_BANCO);

                    if (package != "pkg_webserv_update_bsnotas.PROC_UPD_PCPRODUT")
                    {
                        dynamicParameters.Add("pDTINSERT", prod.DT_INSERT);
                    }

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    produtosJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    produtosJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }

            return produtosJson;
        }
        public async Task<List<Message>> FornecedorAsync(List<Message> fornecedoresJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in fornecedoresJson)
            {
                try
                {
                    var f = JsonConvert.DeserializeObject<Fornecedor>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pROWID", f.ROWID_TB);
                    dynamicParameters.Add("pCODFORNEC", f.CODFORNEC);
                    dynamicParameters.Add("pFORNECEDOR", f.FORNECEDOR);
                    dynamicParameters.Add("pCNPJ", f.CNPJ);
                    dynamicParameters.Add("pESTADO", f.ESTADO);
                    dynamicParameters.Add("pDEPARTAMENTO", f.DEPARTAMENTO);
                    dynamicParameters.Add("pBLOQUEIOSEFAZFORNEC", f.BLOQUEIOSEFAZFORNEC);
                    dynamicParameters.Add("pREVENDA", f.REVENDA);
                    dynamicParameters.Add("pBLOQUEIO", f.BLOQUEIO);
                    dynamicParameters.Add("pPRAZOENTREGA", f.PRAZOENTREGA);
                    dynamicParameters.Add("pCODFORNECPRINC", f.CODFORNECPRINC);
                    dynamicParameters.Add("pCODCONTAB", f.CODCONTAB);
                    dynamicParameters.Add("pSEQ_CLIENTE", f.SEQ_CLIENTE);
                    dynamicParameters.Add("pSTRING_BANCO", f.STRING_BANCO);

                    if (package != ("pkg_webserv_update_bsnotas.PROC_UPD_PCFORNEC")) dynamicParameters.Add("pDTINSERT", f.DT_INSERT);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    fornecedoresJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    fornecedoresJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                }
            }
            return fornecedoresJson;
        }
        public async Task<List<Message>> XmlAsync(List<Message> xmlJson, string package)
        {
            using OracleConnection conn = new OracleConnection(_config.Connection.ConnectionString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            foreach (var item in xmlJson)
            {
                try
                {
                    var x = JsonConvert.DeserializeObject<ArquivoXml>(item.Content);

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pCONTEUDO", x.Conteudo, dbType: OracleMappingType.Clob);
                    dynamicParameters.Add("pNOME_ARQUIVO", x.NOME_ARQUIVO);
                    dynamicParameters.Add("pSEQ_CLIENTE", x.SEQ_CLIENTE);
                    dynamicParameters.Add("pSTRING_BANCO", x.STRING_BANCO);
                    dynamicParameters.Add("pNUM_LOTE_NFE", x.NUM_LOTE_NFE);
                    dynamicParameters.Add("pCHAVENFE", x.CHAVENFE);
                    dynamicParameters.Add("pORIGEM", x.ORIGEM);

                    await conn.QueryAsync(package, param: dynamicParameters, commandType: CommandType.StoredProcedure);

                    xmlJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);

                    if (e.Message.Contains("ORA-06512"))
                    {
                        xmlJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = true);
                    }
                    else
                    {
                        xmlJson.Where(x => x.DeliveryTag == item.DeliveryTag).ToList().ForEach(n => n.Executado = false);
                    }
                }
            }
            return xmlJson;
        }

    }
}

