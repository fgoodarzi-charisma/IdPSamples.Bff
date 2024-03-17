FROM dockerhost.charisma.tech/node:21-alpine3.18
WORKDIR /app
COPY . .

ENV TARGET=http://localhost:8080

EXPOSE 7003
EXPOSE 80

RUN npm install

ENTRYPOINT ["npm", "start"]