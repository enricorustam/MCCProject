CREATE PROCEDURE SP_DeleteForm
	@Id int 
AS
	delete from TB_M_Form where Id = @Id;
RETURN 0