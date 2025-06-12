@echo off
echo Iniciando aplicacoes...

REM Iniciar UserAPI
start "UserAPI" cmd /k "cd backend\userapi && dotnet run"

REM Iniciar CurrencyAPI
start "CurrencyAPI" cmd /k "cd backend\currencyapi && dotnet watch run"

REM Iniciar Frontend (React)
start "Frontend" cmd /k "cd frontend && npm run dev"

echo Todas as aplicacoes foram iniciadas em terminais separados.
pause
