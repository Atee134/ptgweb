﻿/* tslint:disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v12.0.12.0 (NJsonSchema v9.13.15.0 (Newtonsoft.Json v11.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming



export class CreateGameSessionRequestDto implements ICreateGameSessionRequestDto {
    playerName?: string | null;

    constructor(data?: ICreateGameSessionRequestDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.playerName = data["PlayerName"] !== undefined ? data["PlayerName"] : <any>null;
        }
    }

    static fromJS(data: any): CreateGameSessionRequestDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateGameSessionRequestDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PlayerName"] = this.playerName !== undefined ? this.playerName : <any>null;
        return data; 
    }

    clone(): CreateGameSessionRequestDto {
        const json = this.toJSON();
        let result = new CreateGameSessionRequestDto();
        result.init(json);
        return result;
    }
}

export interface ICreateGameSessionRequestDto {
    playerName?: string | null;
}

export class DiamondSquareHeightmapRequestDto implements IDiamondSquareHeightmapRequestDto {
    size!: number;
    offsetRange!: number;
    offsetReductionRate!: number;

    constructor(data?: IDiamondSquareHeightmapRequestDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.size = data["Size"] !== undefined ? data["Size"] : <any>null;
            this.offsetRange = data["OffsetRange"] !== undefined ? data["OffsetRange"] : <any>null;
            this.offsetReductionRate = data["OffsetReductionRate"] !== undefined ? data["OffsetReductionRate"] : <any>null;
        }
    }

    static fromJS(data: any): DiamondSquareHeightmapRequestDto {
        data = typeof data === 'object' ? data : {};
        let result = new DiamondSquareHeightmapRequestDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Size"] = this.size !== undefined ? this.size : <any>null;
        data["OffsetRange"] = this.offsetRange !== undefined ? this.offsetRange : <any>null;
        data["OffsetReductionRate"] = this.offsetReductionRate !== undefined ? this.offsetReductionRate : <any>null;
        return data; 
    }

    clone(): DiamondSquareHeightmapRequestDto {
        const json = this.toJSON();
        let result = new DiamondSquareHeightmapRequestDto();
        result.init(json);
        return result;
    }
}

export interface IDiamondSquareHeightmapRequestDto {
    size: number;
    offsetRange: number;
    offsetReductionRate: number;
}

export class FaultHeightmapRequestDto implements IFaultHeightmapRequestDto {
    width!: number;
    height!: number;
    iterationCount!: number;
    offsetPerIteration!: number;

    constructor(data?: IFaultHeightmapRequestDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.width = data["Width"] !== undefined ? data["Width"] : <any>null;
            this.height = data["Height"] !== undefined ? data["Height"] : <any>null;
            this.iterationCount = data["IterationCount"] !== undefined ? data["IterationCount"] : <any>null;
            this.offsetPerIteration = data["OffsetPerIteration"] !== undefined ? data["OffsetPerIteration"] : <any>null;
        }
    }

    static fromJS(data: any): FaultHeightmapRequestDto {
        data = typeof data === 'object' ? data : {};
        let result = new FaultHeightmapRequestDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Width"] = this.width !== undefined ? this.width : <any>null;
        data["Height"] = this.height !== undefined ? this.height : <any>null;
        data["IterationCount"] = this.iterationCount !== undefined ? this.iterationCount : <any>null;
        data["OffsetPerIteration"] = this.offsetPerIteration !== undefined ? this.offsetPerIteration : <any>null;
        return data; 
    }

    clone(): FaultHeightmapRequestDto {
        const json = this.toJSON();
        let result = new FaultHeightmapRequestDto();
        result.init(json);
        return result;
    }
}

export interface IFaultHeightmapRequestDto {
    width: number;
    height: number;
    iterationCount: number;
    offsetPerIteration: number;
}

export class JoinGameSessionRequestDto implements IJoinGameSessionRequestDto {
    sessionId!: string;
    playerName?: string | null;

    constructor(data?: IJoinGameSessionRequestDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.sessionId = data["SessionId"] !== undefined ? data["SessionId"] : <any>null;
            this.playerName = data["PlayerName"] !== undefined ? data["PlayerName"] : <any>null;
        }
    }

    static fromJS(data: any): JoinGameSessionRequestDto {
        data = typeof data === 'object' ? data : {};
        let result = new JoinGameSessionRequestDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["SessionId"] = this.sessionId !== undefined ? this.sessionId : <any>null;
        data["PlayerName"] = this.playerName !== undefined ? this.playerName : <any>null;
        return data; 
    }

    clone(): JoinGameSessionRequestDto {
        const json = this.toJSON();
        let result = new JoinGameSessionRequestDto();
        result.init(json);
        return result;
    }
}

export interface IJoinGameSessionRequestDto {
    sessionId: string;
    playerName?: string | null;
}

export class RandomHeightmapRequestDto implements IRandomHeightmapRequestDto {
    width!: number;
    height!: number;

    constructor(data?: IRandomHeightmapRequestDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.width = data["Width"] !== undefined ? data["Width"] : <any>null;
            this.height = data["Height"] !== undefined ? data["Height"] : <any>null;
        }
    }

    static fromJS(data: any): RandomHeightmapRequestDto {
        data = typeof data === 'object' ? data : {};
        let result = new RandomHeightmapRequestDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Width"] = this.width !== undefined ? this.width : <any>null;
        data["Height"] = this.height !== undefined ? this.height : <any>null;
        return data; 
    }

    clone(): RandomHeightmapRequestDto {
        const json = this.toJSON();
        let result = new RandomHeightmapRequestDto();
        result.init(json);
        return result;
    }
}

export interface IRandomHeightmapRequestDto {
    width: number;
    height: number;
}

export class HeightmapDto implements IHeightmapDto {
    id!: string;
    width!: number;
    height!: number;
    heightmapFloatArray?: number[] | null;
    heightmapByteArray?: string | null;

    constructor(data?: IHeightmapDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.id = data["Id"] !== undefined ? data["Id"] : <any>null;
            this.width = data["Width"] !== undefined ? data["Width"] : <any>null;
            this.height = data["Height"] !== undefined ? data["Height"] : <any>null;
            if (data["HeightmapFloatArray"] && data["HeightmapFloatArray"].constructor === Array) {
                this.heightmapFloatArray = [] as any;
                for (let item of data["HeightmapFloatArray"])
                    this.heightmapFloatArray!.push(item);
            }
            this.heightmapByteArray = data["HeightmapByteArray"] !== undefined ? data["HeightmapByteArray"] : <any>null;
        }
    }

    static fromJS(data: any): HeightmapDto {
        data = typeof data === 'object' ? data : {};
        let result = new HeightmapDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Id"] = this.id !== undefined ? this.id : <any>null;
        data["Width"] = this.width !== undefined ? this.width : <any>null;
        data["Height"] = this.height !== undefined ? this.height : <any>null;
        if (this.heightmapFloatArray && this.heightmapFloatArray.constructor === Array) {
            data["HeightmapFloatArray"] = [];
            for (let item of this.heightmapFloatArray)
                data["HeightmapFloatArray"].push(item);
        }
        data["HeightmapByteArray"] = this.heightmapByteArray !== undefined ? this.heightmapByteArray : <any>null;
        return data; 
    }

    clone(): HeightmapDto {
        const json = this.toJSON();
        let result = new HeightmapDto();
        result.init(json);
        return result;
    }
}

export interface IHeightmapDto {
    id: string;
    width: number;
    height: number;
    heightmapFloatArray?: number[] | null;
    heightmapByteArray?: string | null;
}

export class SplatmapDto implements ISplatmapDto {
    id!: string;
    width!: number;
    height!: number;
    splatmapByteArray?: string | null;

    constructor(data?: ISplatmapDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.id = data["Id"] !== undefined ? data["Id"] : <any>null;
            this.width = data["Width"] !== undefined ? data["Width"] : <any>null;
            this.height = data["Height"] !== undefined ? data["Height"] : <any>null;
            this.splatmapByteArray = data["SplatmapByteArray"] !== undefined ? data["SplatmapByteArray"] : <any>null;
        }
    }

    static fromJS(data: any): SplatmapDto {
        data = typeof data === 'object' ? data : {};
        let result = new SplatmapDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Id"] = this.id !== undefined ? this.id : <any>null;
        data["Width"] = this.width !== undefined ? this.width : <any>null;
        data["Height"] = this.height !== undefined ? this.height : <any>null;
        data["SplatmapByteArray"] = this.splatmapByteArray !== undefined ? this.splatmapByteArray : <any>null;
        return data; 
    }

    clone(): SplatmapDto {
        const json = this.toJSON();
        let result = new SplatmapDto();
        result.init(json);
        return result;
    }
}

export interface ISplatmapDto {
    id: string;
    width: number;
    height: number;
    splatmapByteArray?: string | null;
}

export class PlayerDto implements IPlayerDto {
    id!: number;
    name?: string | null;
    sessionId!: string;

    constructor(data?: IPlayerDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.id = data["Id"] !== undefined ? data["Id"] : <any>null;
            this.name = data["Name"] !== undefined ? data["Name"] : <any>null;
            this.sessionId = data["SessionId"] !== undefined ? data["SessionId"] : <any>null;
        }
    }

    static fromJS(data: any): PlayerDto {
        data = typeof data === 'object' ? data : {};
        let result = new PlayerDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Id"] = this.id !== undefined ? this.id : <any>null;
        data["Name"] = this.name !== undefined ? this.name : <any>null;
        data["SessionId"] = this.sessionId !== undefined ? this.sessionId : <any>null;
        return data; 
    }

    clone(): PlayerDto {
        const json = this.toJSON();
        let result = new PlayerDto();
        result.init(json);
        return result;
    }
}

export interface IPlayerDto {
    id: number;
    name?: string | null;
    sessionId: string;
}

export class StartGameSesionRequestDto implements IStartGameSesionRequestDto {
    sessionId!: string;
    terrainDataId!: string;

    constructor(data?: IStartGameSesionRequestDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.sessionId = data["SessionId"] !== undefined ? data["SessionId"] : <any>null;
            this.terrainDataId = data["TerrainDataId"] !== undefined ? data["TerrainDataId"] : <any>null;
        }
    }

    static fromJS(data: any): StartGameSesionRequestDto {
        data = typeof data === 'object' ? data : {};
        let result = new StartGameSesionRequestDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["SessionId"] = this.sessionId !== undefined ? this.sessionId : <any>null;
        data["TerrainDataId"] = this.terrainDataId !== undefined ? this.terrainDataId : <any>null;
        return data; 
    }

    clone(): StartGameSesionRequestDto {
        const json = this.toJSON();
        let result = new StartGameSesionRequestDto();
        result.init(json);
        return result;
    }
}

export interface IStartGameSesionRequestDto {
    sessionId: string;
    terrainDataId: string;
}