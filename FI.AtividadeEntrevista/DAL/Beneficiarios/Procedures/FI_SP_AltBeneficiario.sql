CREATE PROC FI_SP_AltBeneficiario
    @NOME          VARCHAR (50) ,
	@CPF           VARCHAR (14) ,
	@IdCliente     BIGINT,
	@Id            BIGINT
AS
BEGIN
	UPDATE BENEFICIARIOS 
	SET 
		NOME = @NOME, 
		CPF = @CPF,
		IdCliente = @IdCliente
	WHERE Id = @Id
END