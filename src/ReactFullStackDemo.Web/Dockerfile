FROM node:20-alpine AS build
WORKDIR /app
COPY src/ReactFullStackDemo.Web/package*.json ./
RUN npm ci
COPY src/ReactFullStackDemo.Web/ ./
RUN npm run build

FROM nginx:1.27-alpine
COPY src/ReactFullStackDemo.Web/nginx.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
