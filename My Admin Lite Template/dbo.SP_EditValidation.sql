CREATE PROCEDURE SP_EditValidation
	@Id int,
	@Action varchar(50),
	@supervisor int,
	@form int
AS
	Update TB_M_Validation
	set
		Action = @Action,
		supervisorId = @supervisor,
		formId = @form
	where Id = @Id
RETURN 0