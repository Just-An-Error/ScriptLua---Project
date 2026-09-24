import { inject, Injectable } from "@angular/core";
import { AppConfig } from "../models/config-model";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private _config: AppConfig = {
    apiUrl: ''
  }

  private readonly _http = inject(HttpClient);

  async loadConfig(): Promise<void> {
    const config = await firstValueFrom(this._http.get<AppConfig>('config.json'));
    this._config = config;
  }

  get settings(): AppConfig {
    return this._config;
  }
}
