# Etapa 1: build
FROM node:20 AS build
WORKDIR /app

# Copia package.json e package-lock.json
COPY package*.json ./

# Instala dependências
RUN npm install

# Copia o restante do código
COPY . .

# Build da aplicação Next.js
RUN npm run build

# Etapa 2: runtime
FROM node:20-alpine AS runtime
WORKDIR /app

# Copia apenas os arquivos necessários do build
COPY --from=build /app/package*.json ./
COPY --from=build /app/.next ./.next
COPY --from=build /app/public ./public
COPY --from=build /app/node_modules ./node_modules

# Expõe a porta padrão do Next.js
EXPOSE 3000

# Comando para rodar a aplicação
CMD ["npm", "run", "start"]
