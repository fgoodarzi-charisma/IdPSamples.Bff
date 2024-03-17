const { env } = require("process");

const target = env.TARGET || "http://localhost:6001";

const PROXY_CONFIG = [
  {
    context: ["/bff", "/signin-oidc", "signout-callback-oidc", "/plans"],
    target,
    secure: false,
  },
];

module.exports = PROXY_CONFIG;
