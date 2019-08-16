Shader "Unlit/Grass1"
{
	static const GLchar* szGrassShader =
		"																							\n\
#ifdef GL_ES																						\n\
    precision mediump float;																		\n\
#endif																								\n\
\n\
varying vec2 v_texCoord;																			\n\
uniform sampler2D u_texture;																		\n\
uniform float u_time;																				\n\
\n\
// 1																								\n\
const float speed = 2.0;																			\n\
const float bendFactor = 0.2;																		\n\
void main()																							\n\
{																									\n\
    // 获得高度，texCoord从下到上为0到1																\n\
    float height = 1.0 - v_texCoord.y;																\n\
    // 获得偏移量，一个幂函数，值愈大，导数越大，偏移量愈大											\n\
    float offset = pow(height, 2.5);																\n\
    // 偏移量随时间变化，并乘以幅度，设置频率														\n\
    offset *= (sin(u_time * speed) * bendFactor);													\n\
    // 使x坐标偏移，fract取区间值（0，1）															\n\
    vec3 normalColor = texture2D(u_texture, fract(vec2(v_texCoord.x + offset, v_texCoord.y))).rgb;	\n\
    gl_FragColor = vec4(normalColor, 1);															\n\
}";
	virtual void update(float dt);
	int m_nTimeUniformLocation;
	float m_fTime;
	LayerColor* bgColor = LayerColor::create(Color4B::WHITE);
	addChild(bgColor);


	Sprite* pSprite = Sprite::create("elephant1_Diffuse.png");
	Size winSize = Director::getInstance()->getWinSize();
	pSprite->setPosition(Vec2(size.width / 4, size.height / 4));
	pSprite->setTag(10001);
	this->addChild(pSprite);

	// 加载顶点着色器和片元着色器
	GLProgram* pShader = new  GLProgram();
	pShader->initWithByteArrays(ccPositionTextureA8Color_vert, szGrassShader);//顶点着色器，后一个参数则指定了像素着色器：
	pSprite->setGLProgram(pShader);
	pShader->release();
	CHECK_GL_ERROR_DEBUG();

	// 启用顶点着色器的attribute变量，坐标、纹理坐标、颜色
	pSprite->getGLProgram()->bindAttribLocation(GLProgram::ATTRIBUTE_NAME_POSITION, GLProgram::VERTEX_ATTRIB_POSITION);//对应vs里面的顶点坐标
	pSprite->getGLProgram()->bindAttribLocation(GLProgram::ATTRIBUTE_NAME_COLOR, GLProgram::VERTEX_ATTRIB_COLOR);//对应vs里面的顶点颜色
	pSprite->getGLProgram()->bindAttribLocation(GLProgram::ATTRIBUTE_NAME_TEX_COORD2, GLProgram::VERTEX_ATTRIB_TEX_COORD);//对应vs里面的顶点纹理坐标
	CHECK_GL_ERROR_DEBUG();

	// 自定义着色器链接
	pSprite->getGLProgram()->link();//因为绑定了属性，所以需要link一下，否则vs无法识别属性
	CHECK_GL_ERROR_DEBUG();

	// 设置移动、缩放、旋转矩阵
	pSprite->getGLProgram()->updateUniforms();//绑定了纹理贴图
	CHECK_GL_ERROR_DEBUG();

	m_nTimeUniformLocation = glGetUniformLocation(pSprite->getGLProgram()->getProgram(), "u_time");

	pSprite->getGLProgram()->use();//调用glUseProgram()方法

								   // 开启帧更新
	this->scheduleUpdate();
	void HelloWorld::update(float delta)
	{
		m_fTime += delta;
		Sprite* p = (Sprite*)getChildByTag(10001);
		if (p)
		{
			p->getGLProgram()->use();
		}
		glUniform1f(m_nTimeUniformLocation, m_fTime);
	}

}
