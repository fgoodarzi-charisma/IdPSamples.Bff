FROM dockerhost.charisma.tech/node:16.17
WORKDIR /app
COPY . .

RUN npm install

ENV TARGET=http://localhost:8080

EXPOSE 8003

ENTRYPOINT ["npm", "start"]