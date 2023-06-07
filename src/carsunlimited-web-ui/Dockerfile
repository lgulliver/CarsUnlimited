# A dockerfile to build and server a react app

FROM node:16-alpine as build

WORKDIR /app

COPY . .

RUN npm install

RUN npm run build .env.development

FROM nginx:alpine

COPY --from=build /app/build /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]