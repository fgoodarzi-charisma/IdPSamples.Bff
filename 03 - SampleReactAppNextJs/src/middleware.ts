import { NextRequest, NextResponse } from 'next/server'

export default async function middleware(request: NextRequest) {
  const pathes = ['/bff', '/signin-oidc', '/signout-callback-oidc', '/plans'];

  request.headers.append('X-Host', 'localhost:7003');
  request.headers.append('X-CSRF', '1');
  const { pathname, search } = new URL(request.url)

  if (pathes.some(path => request.nextUrl.pathname.startsWith(path)))
    return NextResponse.rewrite(new URL(`${pathname}${search||''}`, 'http://localhost:6001'), {
      request: {
        headers: request.headers,
      }
    })
}