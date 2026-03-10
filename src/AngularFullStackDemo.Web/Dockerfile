FROM node:20-alpine AS build
WORKDIR /app
COPY src/AngularFullStackDemo.Web/package*.json ./
RUN npm ci
COPY src/AngularFullStackDemo.Web/ ./
RUN npm run build

FROM nginx:1.27-alpine
COPY src/AngularFullStackDemo.Web/nginx.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist/AngularFullStackDemo.Web /usr/share/nginx/html
EXPOSE 80
