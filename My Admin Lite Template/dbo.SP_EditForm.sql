CREATE PROCEDURE SP_EditForm
	@Id int,
	@employee int, 
	@StartDate DateTime,
	@EndDate DateTime,
	@Duration int,
	@supervisor int,
	@department int
AS
	Update TB_M_Form
	set
		employeeId = @employee,
		StartDate = @StartDate,
		EndDate = @EndDate,
		Duration = @Duration,
		supervisorId = @supervisor,
		departmentId = @department
	where Id = @Id
RETURN 0