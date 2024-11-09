--CREATE PROCEDURE FI_SP_IncBeneficiario
--    @NOME          VARCHAR (50),
--    @CPF           VARCHAR (14),
--    @IdCliente     BIGINT
--AS
--BEGIN
--    INSERT INTO BENEFICIARIOS (NOME, CPF, IdCliente)
--    VALUES (@NOME, @CPF, @IdCliente)

--    SELECT SCOPE_IDENTITY() AS NewBeneficiarioID
--END


--//2

CREATE PROCEDURE FI_SP_IncBeneficiarioV2
    @NOME          VARCHAR (50),
    @CPF           VARCHAR (14),
    @IdCliente     BIGINT
AS
BEGIN
    -- Verifica se já existe um beneficiário com o mesmo CPF e IdCliente
    IF EXISTS (SELECT 1 FROM BENEFICIARIOS WHERE CPF = @CPF AND IdCliente = @IdCliente)
    BEGIN
        -- Se existir, retorna um erro (pode ser personalizado conforme necessário)
        RAISERROR('Beneficiário com o mesmo CPF já cadastrado para este cliente.', 16, 1);
        RETURN;
    END

    -- Se não existir, insere o novo beneficiário
    INSERT INTO BENEFICIARIOS (NOME, CPF, IdCliente)
    VALUES (@NOME, @CPF, @IdCliente)

    -- Retorna o ID do novo beneficiário inserido
    SELECT SCOPE_IDENTITY() AS NewBeneficiarioID
END