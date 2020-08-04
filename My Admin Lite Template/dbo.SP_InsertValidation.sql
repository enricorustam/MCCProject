CREATE PROCEDURE SP_InsertValidation
	@Action varchar(50),
	@supervisor int,
	@form int
AS
	insert into TB_M_Validation(Action, supervisorId, formId) values(@Action,@supervisor,@form);
return 0