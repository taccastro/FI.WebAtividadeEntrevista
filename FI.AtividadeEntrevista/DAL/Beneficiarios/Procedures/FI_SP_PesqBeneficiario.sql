--CREATE PROCEDURE FI_SP_PesqBeneficiario
--	@iniciarEm int,
--	@quantidade int,
--	@campoOrdenacao varchar(200),
--	@crescente bit	
--AS
--BEGIN
--	DECLARE @SCRIPT NVARCHAR(MAX)
--	DECLARE @CAMPOS NVARCHAR(MAX)
--	DECLARE @ORDER VARCHAR(50)
	
--	IF(@campoOrdenacao = 'EMAIL')
--		SET @ORDER =  ' EMAIL '
--	ELSE
--		SET @ORDER = ' NOME '

--	IF(@crescente = 0)
--		SET @ORDER = @ORDER + ' DESC'
--	ELSE
--		SET @ORDER = @ORDER + ' ASC'

--	SET @CAMPOS = '@iniciarEm int,@quantidade int'
--	SET @SCRIPT = 
--	'SELECT ID, NOME, CPF, IDCLIENTE FROM
--		(SELECT ROW_NUMBER() OVER (ORDER BY ' + @ORDER + ') AS Row, ID, NOME, CPF, IDCLIENTE FROM BENEFICIARIOS WITH(NOLOCK))
--		AS BeneficiariosWithRowNumbers
--	WHERE Row > @iniciarEm AND Row <= (@iniciarEm+@quantidade) ORDER BY'
	
--	SET @SCRIPT = @SCRIPT + @ORDER
			
--	EXECUTE SP_EXECUTESQL @SCRIPT, @CAMPOS, @iniciarEm, @quantidade

--	SELECT COUNT(1) FROM BENEFICIARIOS WITH(NOLOCK)
--END


CREATE PROCEDURE FI_SP_PesqBeneficiario
    @iniciarEm int,
    @quantidade int,
    @campoOrdenacao varchar(200),
    @crescente bit,
    @idCliente int = NULL  -- Parâmetro para o ID do cliente
AS
BEGIN
    DECLARE @SCRIPT NVARCHAR(MAX)
    DECLARE @CAMPOS NVARCHAR(MAX)
    DECLARE @ORDER VARCHAR(50)
    
    IF(@campoOrdenacao = 'EMAIL')
        SET @ORDER =  ' EMAIL '
    ELSE
        SET @ORDER = ' NOME '

    IF(@crescente = 0)
        SET @ORDER = @ORDER + ' DESC'
    ELSE
        SET @ORDER = @ORDER + ' ASC'

    SET @CAMPOS = '@iniciarEm int, @quantidade int, @idCliente int'
    SET @SCRIPT = 
    'SELECT ID, NOME, CPF, IDCLIENTE FROM
        (SELECT ROW_NUMBER() OVER (ORDER BY ' + @ORDER + ') AS Row, ID, NOME, CPF, IDCLIENTE 
         FROM BENEFICIARIOS WITH(NOLOCK)
         WHERE IDCLIENTE = @idCliente) AS BeneficiariosWithRowNumbers
    WHERE Row > @iniciarEm AND Row <= (@iniciarEm + @quantidade) ORDER BY ' + @ORDER

    EXECUTE SP_EXECUTESQL @SCRIPT, @CAMPOS, @iniciarEm, @quantidade, @idCliente

    SELECT COUNT(1) FROM BENEFICIARIOS WITH(NOLOCK)
    WHERE IDCLIENTE = @idCliente
END
