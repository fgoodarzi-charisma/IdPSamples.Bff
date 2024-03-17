import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";
import { env } from "process";

const target = env.TARGET || "http://localhost:6001";

export default defineConfig({
  plugins: [plugin()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    proxy: {
      "^/plans": {
        target,
        secure: false,
      },
      "^/bff": {
        target,
        secure: false,
      },
      "^/signin-oidc": {
        target,
        secure: false,
      },
      "^/signout-callback-oidc": {
        target,
        secure: false,
      },
    },
    port: 7003,
  },
});
