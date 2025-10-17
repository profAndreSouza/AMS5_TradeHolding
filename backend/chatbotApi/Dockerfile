# Usa a imagem oficial do Python 3.14 (slim)
FROM python:3.14-slim

# Define diretório de trabalho
WORKDIR /app

# Copia dependências e instala
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copia o código da aplicação
COPY . .

# Expõe a porta usada pelo Flask
EXPOSE 8080

# Comando de inicialização
CMD ["python", "app.py"]
