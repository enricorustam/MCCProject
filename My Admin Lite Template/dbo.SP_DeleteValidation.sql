CREATE PROCEDURE SP_DeleteValidation
	@Id int 
AS
	delete from TB_M_Validation where Id = @Id;
RETURN 0